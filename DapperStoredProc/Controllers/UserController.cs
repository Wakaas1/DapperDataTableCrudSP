using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
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

namespace DapperStoredProc.Controllers
{

    public class UserController : Controller
    {
       
        private readonly IUserServices _user;
        private readonly IWebHostEnvironment _webHostEnvironment;
       
        
        
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
            if (ModelState.IsValid)
            {

                if (dto.Password == dto.ConfirmPassword)
                {
                    var user = new Users
                    {
                        Name = dto.Name,
                        Email = dto.Email,
                        Password = _user.CreatePasswordHash(dto.Password),
                        Image = dto.Image,
                        Role = dto.Role,
                        Token = dto.Token

                    };
                    _user.AddUser(user);
                }
            }
            else
            {
                return BadRequest("Please enter correct detail.");
            }
            return RedirectToAction("Login");
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            if (ModelState.IsValid)
            {
                var user = new Users();
                var req = _user.GetEmpByEmail(dto.Email);

                if (_user.VerifyPasswordHash(req.Password, dto.Password))
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,Convert.ToString(User.Identity)),

                    };
                    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties());

                    return LocalRedirect("~/");
                }
                }
                else
                {
                    return BadRequest("Password not macth.");
                }
            return LocalRedirect("~/");
                
        }
   
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
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return LocalRedirect("~/");
        }

        private bool CheckPassword(string Password,string Email)
        {
            var user = _user.GetEmpByEmail(Email);

            if(user.Password==Password)
            
                return true;
            return false;
            

        }
    }
}
