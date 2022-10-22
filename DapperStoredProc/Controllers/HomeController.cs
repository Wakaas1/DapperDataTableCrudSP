//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.Extensions.Logging;
//using DapperStoredProc.Models;
//using DapperStoredProc.Services;
//using DapperStoredProc.Data;


//namespace DapperStoredProc.Controllers
//{
//    public class HomeController : Controller
//    {
       
//        private readonly IEmployeeServices _Services;
//        private readonly ApplicationDbContext _context;
//        public HomeController(IEmployeeServices Services, ApplicationDbContext context)
//        {
//            _Services = Services;
//            _context = context;
//        }

//        public IActionResult Index()
//        {
//            return RedirectToAction("Index", "Employee");
//        }
//        public IActionResult Login()
//        {

//            return View();
//        }
//        [HttpPost]
       
//        public IActionResult Login(Models.User user)
//        {
           
//            return View(user);
//        }

//        public IActionResult SignUp()
//        {
//            return View();
//        }
//        [HttpPost]
//        [ValidateAntiForgeryToken]
//        public IActionResult SignUp(UserSignup userSignups)
//        {
//            _context.Add(userSignups);
//            _context.SaveChanges();
//            ViewBag.message = "The user " + userSignups.Name + " is saved successfully";
//            return View();    
//        }

//        public IActionResult UploadImage()
//        {
//            return View();
//        }

//        //[HttpPost]
//        //public IActionResult UploadImage()
//        //{
//        //    return View();
//        //}

//        public IActionResult Logout()
//        {
           
//            return View();
//        }
//    }

//}
