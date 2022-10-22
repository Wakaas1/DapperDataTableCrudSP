using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using DapperStoredProc.Services;
using Microsoft.AspNetCore.Mvc;

namespace DapperStoredProc.Controllers
{

    public class UserController : Controller
    {
         private readonly IUserServices _user;
            public UserController(IUserServices user)
        {
            _user = user;
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
            if(dto.Password == dto.ConfirmPassword)
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
                return BadRequest();
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
                     return RedirectToAction("Index");
                }
                else
                {
                    return BadRequest();
                }
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
