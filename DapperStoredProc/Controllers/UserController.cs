using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using DapperStoredProc.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperStoredProc.Controllers
{

    public class UserController : Controller
    {
        private readonly IEmailServices _email;
        private readonly IUserServices _user;
        private readonly IRoleServices _role;
        private readonly IWebHostEnvironment _webHostEnvironment;
        
       
        private class ICustomICustomClaimsCookieSignInHelper { }
        public UserController(IUserServices user, IWebHostEnvironment webHostEnvironment, IRoleServices role, IEmailServices email)
        {
            _user = user;
            _webHostEnvironment = webHostEnvironment;
            _role = role;
            _email = email;

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
            if (ModelState.IsValid)
            {
                System.Guid guid = System.Guid.NewGuid();
                var token = guid.ToString();

                DateTime now = DateTime.UtcNow;

                if (dto.Password == dto.ConfirmPassword)
                {
                    var user = new Users
                    {
                        Name = dto.Name,
                        Email = dto.Email,
                        Password = _user.CreatePasswordHash(dto.Password),
                        Token = token,
                        IsVerify = false,
                        TokenGeneratedDate = now
                    };
                    _user.AddUser(user);
                    var lnkHref = "<a href='" + Url.Action("ConfirmOTP", "User", new { token }, "https") + "'>Email Confirmation</a>";

                    _email.sendEmail(dto.Email,lnkHref);

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



            //if (ModelState == null)
            //{
            //    Random generator = new Random();
            //    string number = generator.Next(1, 10000).ToString("D4");
            //    int roleid = 2;
            //    if (dto.Password == dto.ConfirmPassword)
            //    {

            //        var user = new Users
            //        {
            //            Name = dto.Name,
            //            Email = dto.Email,
            //            Password = _user.CreatePasswordHash(dto.Password),                                             
            //            Token = number,
            //            Role = roleid
            //        };
            //        _user.AddUser(user);
            //        sendEmail(dto.Email, number);

            //        string email = dto.Email;

            //        TempData["Email"] = email;
            //        return RedirectToAction("ConfirmOTP");

            //    }
            //    else
            //    {
            //        return BadRequest("Please enter correct detail.");
            //    }
            //}
        //    return BadRequest("Wrong Credentials!...");
        //}

        

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

        [HttpGet]
        public IActionResult ConfirmOTP(string token)
        {
            OTP otp = new OTP
            {
                Token = token
            };
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ConfirmOTP(OTP otp,string token)
        {
            string email = (string)TempData["Email"];
            var user = _user.GetUserByEmail(email);

            if (user.TokenGeneratedDate.AddMinutes(30) >= DateTime.UtcNow.AddMinutes(0))
            {
                if (user.Token == token)
                {
                    bool verify = true;
                    _user.UserIsVerified(email, verify);
                    if (user.IsVerify = true)
                    {
                        return LocalRedirect("~/");
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
            }
            return BadRequest("Email Confirmation Link has been expired. kindly resend the link.");
        }
        public IActionResult Login()
        {
            return View();
        }
      

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var req = _user.GetUserByEmail(dto.Email);
            int userId = req.id;
            if (req.IsVerify == true)
            {
                var userrole = _user.UserListId(userId).ToArray();

                if (/*_user.VerifyPasswordHash(req.Password, dto.Password)*/ModelState.IsValid)
                {
                    var claims = new List<Claim>

                    {
                        new Claim(ClaimTypes.Name, req.Email),

                    };

                    foreach (var item in userrole)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item.RName));
                    }
                    var identity = new ClaimsIdentity(claims, "DDLO");

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                              new ClaimsPrincipal(identity),
                              new AuthenticationProperties
                              {
                                  ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                                  IsPersistent = true
                              });
                    return LocalRedirect("~/");
                }
            }
            else
            {
                return BadRequest("User not verified");
            }
            return View();
        }
  public IActionResult Profile()
        {
            var email = HttpContext.Response.HttpContext.User.Identity.Name;
            var user = _user.GetUserByEmail(email);
          
            return View();
        }

        [HttpGet]
        public IActionResult DeleteUser(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            _user.DeleteUser(id.GetValueOrDefault());
            return RedirectToAction("Index", "Employee");
        }
        
        [HttpPost]
        public IActionResult DeleteUser(int id, Users user)
        {
            if (_user.DeleteUser(id) > 0)
            {
                return RedirectToAction("Index", "Employee");
            }
            return View(user);
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
                        
                        _email.sendEmail(email, lnkHref);
                    }
                   
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
                var email = HttpContext.Response.HttpContext.User.Identity.Name;
                var userId = _user.GetUserByEmail(email).id;
                var pathToDB = @"~\wwwroot\Images\" + fileName;
                var user = new Users
                {
                    id = userId,
                    Image = pathToDB
                    
                };
                _user.UpadateUserImage(user);
                

                TempData["Success"] = "The product has been added!";
            }
                return RedirectToAction("Index", "Employee");
            
        }
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return LocalRedirect("~/");
        }

        public IActionResult ProfileView()
        {
            var email = HttpContext.Response.HttpContext.User.Identity.Name;
            var user = _user.GetUserByEmail(email);
            var pro = new ProfileView
            {
                Name = user.Name,
                Email = user.Email,
                Image = user.Image
            };

            return View(pro);
        }
    }
}
