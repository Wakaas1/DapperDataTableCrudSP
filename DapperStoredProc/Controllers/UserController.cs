using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using DapperStoredProc.Models.DataTable;
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

        [Authorize(Roles = "Admin,User,Ceo,Editor,Manager,AM,Director")]
        public IActionResult Index(UserDetail uD)
        {
            var user = _user.GetAllUsers(uD).ToList();


            return View(user);
        }
        [HttpGet]
        public IActionResult GetUserByID(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var user = _user.GetUserByID(id.GetValueOrDefault());
            if (user == null)

                return NotFound();

            return View(user);
        }

        // User Registeration
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterDto dto)
        {

            string date = DateTime.UtcNow.ToString();


            string imageName = "User.png";
            if (dto.UploadImage != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "/User/Img");
                imageName = Guid.NewGuid().ToString() + "_" + dto.UploadImage.FileName + date;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                 dto.UploadImage.CopyToAsync(fs);
                fs.Close();
            }

            dto.Image = imageName;

            

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
                        TokenGeneratedDate = now,
                        Image = imageName
                    };
                    _user.AddUser(user);
                    string lnkHref = "" + Url.Action("ConfirmOTP", "User", new { token }, "https") + "";

                    _email.SendEmail(dto.Email,lnkHref,dto.Name);

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

        // User exist same name in db
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

        //Confirmation Link email send
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

            if (user.TokenGeneratedDate.AddMinutes(30) < DateTime.UtcNow.AddMinutes(30))
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

        // Login

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
        private void SignInUser(Users currentUser, bool isPersistent)
        {
            //Initialization
            var claims = new List<Claim>();

            try
            {   
                //setting
                claims.Add(new Claim(ClaimTypes.Name, currentUser.Name));
                claims.Add(new Claim(ClaimTypes.NameIdentifier, currentUser.id.ToString()));
                //custom claims
                claims.Add(new Claim("id", currentUser.id.ToString()));
                claims.Add(new Claim("Name", currentUser.Name));
                claims.Add(new Claim("Email", currentUser.Email));
                claims.Add(new Claim("Image", currentUser.Image.ToString()));
                // Id Profile Picutue
                   
                
                var identity = new ClaimsIdentity(claims, "DDLO");

                 HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                          new ClaimsPrincipal(identity),
                          new AuthenticationProperties
                          {
                              ExpiresUtc = DateTime.UtcNow.AddMinutes(10),
                              IsPersistent = true
                          });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void ClaimIdentities(string username, bool isPersistent)
        {
            //Initialization
            var claims = new List<Claim>();
            try
            {
                //setting
                claims.Add(new Claim(ClaimTypes.Name, username));
                var ClaimIdenties = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult Profile()
        {
              var email = HttpContext.Response.HttpContext.User.Identity.Name;
              var user = _user.GetUserByEmail(email);
          
               return View();
        }

        //Forgot Password 

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
                        
                        _email.SendEmail(email, lnkHref,user.Name);
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


        //Reset Password 

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


        // Change Password after login 
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
        public IActionResult ImageUpload(ImageModel dto)
        {
            var email = HttpContext.Response.HttpContext.User.Identity.Name;
            var userimg = _user.GetUserByEmail(email).Image;
            string date = DateTime.UtcNow.ToString();
            
            if (dto.ImageFile != null)
            {
                string uploadsDir = Path.Combine(_webHostEnvironment.WebRootPath, "/User/Img");

                if (!string.Equals(userimg, "User.png"))
                {
                    string oldImagePath = Path.Combine(uploadsDir, userimg);
                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }
                
                string imageName = Guid.NewGuid().ToString() + "" + dto.ImageFile.FileName + "" + date;
                string filePath = Path.Combine(uploadsDir, imageName);
                FileStream fs = new FileStream(filePath, FileMode.Create);
                dto.ImageFile.CopyToAsync(fs);
                fs.Close();
                dto.ImageName = imageName;
            }
            
            var userid = _user.GetUserByEmail(email).id;
            _user.UpadateUserImage(dto.ImageName, userid);
            return View();

        }


        //public IActionResult ImageUpload(List<IFormFile> postedFiles)
        //{

        //    string path = Path.Combine(_webHostEnvironment.WebRootPath, "Images");
        //    if (!Directory.Exists(path))
        //    {
        //        Directory.CreateDirectory(path);
        //    }

        //    List<string> uploadedFiles = new List<string>();
        //    foreach (IFormFile postedFile in postedFiles)
        //    {
        //        string fileName = Path.GetFileName(postedFile.FileName);
        //        string imagePath = Path.Combine(path, fileName);
        //        using (FileStream stream = new FileStream(imagePath, FileMode.Create))
        //        {
        //            postedFile.CopyTo(stream);
        //            uploadedFiles.Add(fileName);
        //            ViewBag.Message += string.Format("<b>{0}</b> Images.<br />", fileName);
        //        }
        //        var email = HttpContext.Response.HttpContext.User.Identity.Name;
        //        var userId = _user.GetUserByEmail(email).id;
        //        var pathToDB = @"~\wwwroot\Images\" + fileName;
        //        var user = new Users
        //        {
        //            id = userId,
        //            Image = pathToDB
                    
        //        };
        //        _user.UpadateUserImage(user);
                

        //        TempData["Success"] = "The product has been added!";
        //    }
        //        return RedirectToAction("Index", "Employee");
            
        //}
        [HttpGet]
        public IActionResult CreateImage()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        //public IActionResult CreateImage([Bind("ImageId,Title,ImageName")] ImageModel imageModel)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //Save image to wwwroot/image
        //        string wwwRootPath = _webHostEnvironment.WebRootPath;
        //        string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
        //        string extension = Path.GetExtension(imageModel.ImageFile.FileName);
        //        imageModel.ImageName = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
        //        string path = Path.Combine(wwwRootPath , fileName);
        //        using (var fileStream = new FileStream(path, FileMode.Create))
        //        {
        //            imageModel.ImageFile.CopyTo(fileStream);
        //        }
        //        var email = HttpContext.Response.HttpContext.User.Identity.Name;
        //        var userId = _user.GetUserByEmail(email).id;
        //        var pathToDB = @"~\wwwroot\Images\" + fileName;
        //        var user = new Users
        //        {
        //            id = userId,
        //            Image = pathToDB

        //        };
        //        _user.UpadateUserImage(user);
        //        //var deleteImg = (User.Identity as ClaimsIdentity).Claims.Where(c => c.Type == "Image").FirstOrDefault();
        //        //if (deleteImg != null)
        //        //{
        //        //    string oldimg = Request.Path(deleteImg.Value.ToString());
        //        //    if (System.IO.File.Exists(oldimg))
        //        //    {
        //        //        System.IO.File.Delete(oldimg);
        //        //    }
        //        //}
        //        ViewBag.Message = "profile picture updated successfully.";

        //    }
        //    return RedirectToAction("Index", "Employee");
        //}

        //public IActionResult DeleteImage(int? id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public IActionResult DeleteConfirmed(int id)
        //{
        //    var imageModel =  _user.GetUserByID(id).Image;

        //    //delete image from wwwroot/image
        //    var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, "Image", imageModel.);
        //    if (System.IO.File.Exists(imagePath))
        //        System.IO.File.Delete(imagePath);
         
        //    _user.DeleteUserImage(imageModel);
        //    return RedirectToAction("Index","Employee");
        //}
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


        // User Roles



        // Update User

        [HttpGet]
        public IActionResult UpdateUser(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = _user.GetUserByID(id.GetValueOrDefault());
            if (user == null)

                return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult UpdateUser(int id, [Bind("id,Name,Email,")] Users user)
        {
            long result = 0;
            int Status;
            string Value;
            
            if (ModelState.IsValid)
            {
                result = _user.UpdateUser(user);
                if (result > 0)
                {
                    Status = 200;
                    Value = Url.Content("~/Design/View/");
                }
                else
                {
                    Status = 500;
                    Value = "There is some error at server side";
                }
            }
            else
            {
                Status = 500;
                Value = "There is some error at client side";
            }
            return Json(new { status = Status, value = Value });
        }


        //Delete user
        [HttpGet]
        public IActionResult DeleteUser()
        {
            return View();
       
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            if (_user.DeleteUser(id) > 0)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        //Data Table, Searching, sorting, Paging, Total Count,Filtering
        public JsonResult GetAllUser()
        {
            var request = new DataTableRequest();
            request.Draw = Convert.ToInt32(Request.Form["draw"].FirstOrDefault());
            request.Start = Convert.ToInt32(Request.Form["start"].FirstOrDefault());
            request.Length = Convert.ToInt32(Request.Form["length"].FirstOrDefault());
            request.Search = new DataTableSearch()
            {
                Value = Request.Form["search[value]"].FirstOrDefault()
            };
            request.Order = new DataTableOrder[] {
            new DataTableOrder()
            {
                Dir = Request.Form["order[0][dir]"].FirstOrDefault(),
               Column = Convert.ToInt32(Request.Form["order[0][column]"].FirstOrDefault())
            }};
            return Json(_user.GetAllUserDT(request).Result);
        }


    }
}
