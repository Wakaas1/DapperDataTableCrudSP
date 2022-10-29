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
                        Token=number
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
                        new Claim(ClaimTypes.NameIdentifier, req.Name),
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
        public IActionResult ForgotPassword(string email)
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
                        //Create URL with above token
                        var lnkHref = "<a href='" + Url.Action("ResetPassword", "User", new { user.Email, token }, "https") + "'>Reset Password</a>";
                        sendEmail(email, lnkHref);
                        
                    }

                    TempData["tok"] = token;
                    //TempData["Email"] = email;
                    

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

        public IActionResult ResetPassword()
        {
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult ResetPassword(ResetPassword reset)
        //{
        //    //var token = HttpContext.Session.GetString(token);
        //    //var email = HttpContext.Session.GetString(Email);
        //    Guid Token = (Guid)TempData["tok"];
        //    //string Email = (string)TempData["Email"];

        //    if (token == Token.ToString())
        //    {
        //        if (reset.NewPassword == reset.ConfirmPassword)
        //        {
        //            _user.UpdatePassword(email, reset.NewPassword);
        //        }
        //        else
        //        {
        //            return BadRequest("Password does not match.");
        //        }
        //    }
        //    else
        //    {
        //        return BadRequest("OOP! Invalid Token.");
        //    }
        //    return RedirectToAction("Login");
        //}

        [HttpGet]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
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

        //[HttpPost]
        //public IActionResult ImageUpload(User user)
        //{
        //    string imageName = "noimage.jpeg";
        //    if (user.UploadImage != null)
        //    {

        //        string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "wwwroot/Images");
        //        imageName = Guid.NewGuid().ToString() + "_" + user.UploadImage.FileName;
        //        string filePath = Path.Combine(uploadsDir, imageName);
        //        FileStream fs = new FileStream(filePath, FileMode.Create);
        //        user.UploadImage.CopyToAsync(fs);
        //        fs.Close();
        //        user.Image = imageName;
        //    }
        //    //var user = new User();
        //    _context.Add(user);
        //    _context.SaveChanges();

        //    TempData["Success"] = "The product has been added!";

        //    return RedirectToAction("Index");
        //}


        //[HttpPost]
        //public IActionResult ImageUpload(ImageUpload dto, HttpPostedFileBase imgfile)
        //{

        //    string path = Uploadimgfile(imgfile);
        //    if (path.Equals("-1"))
        //    {
        //        ViewBag.error = "Image could not be uploaded....";
        //    }
        //    else
        //    {
        //        var user = new User();

        //        dto.Image = path;
        //        _user.AddUser(user);

        //    }
        //    return RedirectToAction("Index");
        //}


        //public string Uploadimgfile(HttpPostedFileBase file)
        //{
        //    string imageName = "noimage.jpeg";
        //    string path = "-1";
        //    string ticks = DateTime.Now.Ticks.ToString();
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        string extension = Path.GetExtension(file.FileName);
        //        if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
        //        {
        //            try
        //            {
        //                var pathtoreturn = "Images" + ticks + Path.GetFileName(file.FileName);
        //                path = Path.Combine(_webHostEnvironment.WebRootPath,"wwwroot","Image" + ticks + Path.GetFileName(file.FileName));
        //                file.SaveAs(path);
        //                return pathtoreturn;
        //                //    ViewBag.Message = "File uploaded successfully";
        //            }
        //            catch (Exception)
        //            {
        //                path = "-1";
        //            }
        //        }
        //        else
        //        {
        //            ViewBag.Message = "Only jpg ,jpeg or png formats are acceptable....";
        //        }
        //    }
        //    else
        //    {
        //        ViewBag.Message = "Please select a file";
        //        path = "-1";
        //    }


        //    return path;
        //}

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("DDLO");
            return LocalRedirect("~/");
        }

        private bool CheckPassword(string Password,string Email)
        {
            var user = _user.GetUserByEmail(Email);

            if(user.Password==Password)
            
                return true;
            return false;
            

        }

        private class _customClaimsCookieSignInHelper
        {
        }
    }
}
