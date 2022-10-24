using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DapperStoredProc.Data;
using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using DapperStoredProc.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DapperStoredProc.Controllers
{

    public class UserController : Controller
    {
        private HttpPostedFileBase imgfile;
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
        public IActionResult Register(RegisterDto dto)
        {
            if (dto.Password == dto.ConfirmPassword)
            {
                var user = new User
                {
                    Name=dto.Name,
                    Email=dto.Email,
                    Password=dto.Password,
                    Image=dto.Image
                };
                _user.AddUser(user);
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
        public IActionResult Login(LoginDto dto)
        {
                if (CheckPassword(dto.Password, dto.Email)) 
                {
                TempData["Msg"] = "UserName & Password Successfully Updated.";
                return RedirectToAction("Index");
                }
           
                else
                {
                TempData["Msg"] = "Invalid UserName & Password.";
                return RedirectToAction("Index");
                }
        }
   

        [HttpGet]
        public IActionResult ImageUpload()
        {
            return View();
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
                var user = new User
                {
                    Image = imagePath
                };
                _user.UpadateUserImage(user);
                

                TempData["Success"] = "The product has been added!";
            }
                return RedirectToAction("Index");
            
        }

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

        public IActionResult Logout()
        {
            return View();
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
