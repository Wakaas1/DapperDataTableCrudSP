using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using DapperStoredProc.Data;
using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using DapperStoredProc.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMatrix.WebData;

namespace DapperStoredProc.Controllers
{

    public class UserController : Controller
    {
        
        private readonly IUserServices _user;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
       
        private class ICustomICustomClaimsCookieSignInHelper { }
        public UserController(IUserServices user, IWebHostEnvironment webHostEnvironment)
        {
            _user = user;
            _webHostEnvironment = webHostEnvironment;
           


        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto dto)
        {
            if (ModelState == null)
            {
                Random generator = new Random();
                string number = generator.Next(1, 10000).ToString("D4");

                if (dto.Password == dto.ConfirmPassword)
                {
                    var user = new Users
                    {
                        Name = dto.Name,
                        Email = dto.Email,
                        Password = _user.CreatePasswordHash(dto.Password),
                        Image = dto.Image,
                        Role = dto.Role,
                        Token = number
                    };
                    _user.AddUser(user);
                    sendEmail(dto.Email, number);

                    string email = dto.Email;

                    TempData["Email"] = email;
                    return RedirectToAction("ConfirmOTP");

                }
                else
                {
                    return BadRequest("Please enter correct detail.");
                }
            }
            return View();
        }


        [AcceptVerbs("Get", "Post")]
        public IActionResult IsUserAlreadyExists(string email)
        {
            var user = _user.GetUserByEmail(email);
            if (user != null)
            {
                return Json($"Email already exist.");
            }

            else
                return Json(true);
        }
        public IActionResult ConfirmOTP()
        {

            return View();
        }

        [HttpPost]
        public IActionResult ConfirmOTP( string otp)
        {
            string Email = (string)TempData["Email"];
            var user = _user.GetUserByEmail(Email);
            if(user.Token == otp)
            {
               return RedirectToAction("Login");
            }
            else
            {
                return BadRequest("OOP! OTP not match.");
            }
           
        }
        public IActionResult Login()
        {
            return View();
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = new Users();
            if (ModelState.IsValid)
            {
                
                var req = _user.GetUserByEmail(dto.Email);
                //if(dto.Email ==req.Email)
                //{
                    
                    if (_user.VerifyPasswordHash(req.Password, dto.Password))
                    {
                        var claims = new List<Claim>
                        
                    {
                        new Claim(ClaimTypes.Name, req.Email),
                        new Claim(ClaimTypes.Role, req.Role),
                    };
                        //var identity = new ClaimsIdentity(claims, "DDLO");
                       //var principal = new ClaimsPrincipal(identity)

                    var identity = new ClaimsIdentity(claims, "DDLO");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                              new ClaimsPrincipal(identity),
                              new AuthenticationProperties
                              {
                                  ExpiresUtc = DateTime.UtcNow.AddMinutes(1),
                                  IsPersistent = true
                              });
                    return LocalRedirect("~/");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Email & Password not macth.");
                    }
                //}
                //else
                //{
                //    return BadRequest("User not found.");
                //}
                
            }
            return View();
            

        }

        string fromMail = "mw7637@outlook.com";
        string fromMailPassword = "Mianwakaas7637";

        public bool sendEmail(string email, string token)
        {
            if (email == null)
            {
                return false;
            }
            else
            {
                var Token = token;
                var ToEmail = email;
                MailMessage message = new MailMessage(new MailAddress(fromMail, "DapperProcSP"), new MailAddress(ToEmail));
                message.Subject = "EmailConfirmation";
                message.Body = "OTP :" + Token + "thanks";
                message.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp-mail.outlook.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                System.Net.NetworkCredential credential = new System.Net.NetworkCredential();
                credential.UserName = fromMail;
                credential.Password = fromMailPassword;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = credential;
                smtp.Send(message);
                return true;
            }

        }

        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ForgotPassword(ForgotPassword reset, string email )
        {
            if (ModelState.IsValid)
            {
                var user = _user.GetUserByEmail(email);
                if (user!=null)
                {
                   System.Guid guid = System.Guid.NewGuid();
                    var token = guid.ToString();
                    if (token == null)
                    {
                        // If user does not exist or is not confirmed.
                        return BadRequest("Please enter your registered email.");
                    }
                    else
                    {
                        _user.UpdateToken(email,token);

                        //Create URL with above token
                        var lnkHref = "<a href='" + Url.Action("ResetPassword", "User", new { token }, "https") + "'>Reset Password</a>";
                        
                        sendEmail(email, lnkHref);
                    }
                    TempData["tok"] = token;
                    TempData["Email"] = email;
                }
                else
                {
                    // If user does not exist or is not confirmed.
                    return BadRequest("Please enter your registered email.");
                }
            }
            return RedirectToAction("ForgotPasswordConfirmation");
        }
        [HttpGet]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //Reset Password Section
        [HttpGet]
        public IActionResult ResetPassword(string token)
        {
            ResetPassword reset = new ResetPassword
            {
                Token = token

            };
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResetPassword(ResetPassword reset, string token)
        {
            //var token = HttpContext.Session.GetString(token);
            //var email = HttpContext.Session.GetString(Email);
            //Guid Token = (Guid)TempData["tok"];
            string Email = (string)TempData["Email"];
            var user = _user.GetUserByEmail(Email);
            if(user == null)
            {
                return BadRequest("something bad.");
            }
           
            if (user.Token == token)
            {
                if (reset.NewPassword == reset.ConfirmNewPassword)
                {
                    var hash = _user.CreatePasswordHash(reset.NewPassword);
                    _user.UpdatePassword(Email,hash);
                }
                else
                {
                    return BadRequest("Password does not match.");
                }
            }
            else
            {
                return BadRequest("OOP! Invalid Token.");
            }
            
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword()
        {
            return View();
        }
        [HttpPost]
     
        public IActionResult ChangePassword(ChangePassword dto)
        {
           
           if(ModelState.IsValid)
            {
                var email = HttpContext.Response.HttpContext.User.Identity.Name;
                var user = _user.GetUserByEmail(email);
                if(!_user.VerifyPasswordHash(user.Password,dto.OldPassword))
                {
                    return BadRequest("Your password does not macth."); 
                }
                if (dto.NewPassword == dto.ConfirmPassword)
                {
                    var hash = _user.CreatePasswordHash(dto.NewPassword);
                    _user.UpdatePassword(email, hash);
                }
                else
                {
                    return BadRequest("Password does not match.");
                }
                return RedirectToAction("Login");
            }
            else
            {
                return BadRequest("OOP! Wrong Credentials");
            }
        }

       


        //Image Upload

        [HttpGet]
        public IActionResult ImageUpload()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ImageUpload(List<IFormFile> postedFiles)
        {

            string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            List<string> uploadedFiles = new List<string>();
            foreach (IFormFile postedFile in postedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                string imagePath = Path.Combine(path, fileName);
                using (FileStream stream = new FileStream(imagePath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                    uploadedFiles.Add(fileName);
                    ViewBag.Message += string.Format("<b>{0}</b> Images.<br />", fileName);
                }
                var pathToDB = @"~\wwwroot\Images\" + fileName;
                var user = new Users
                {
                    Image = pathToDB,
                    id  = 1
                };
                _user.UpadateUserImage(user);
                

                TempData["Success"] = "The product has been added!";
            }
                return RedirectToAction("Index");
            
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return LocalRedirect("~/");
        }
    }
}
