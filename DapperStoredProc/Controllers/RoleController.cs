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
    
    public class RoleController : Controller
    {
        private readonly IRoleServices _role;
        private readonly IUserServices _user;
        public RoleController(IRoleServices role, IUserServices user)
        {
            _role = role;
            _user = user;
        }
        public IActionResult Index(UserDetail uD)
        {
            var user = _role.GetAllUsers(uD).ToList();

            
           return View(user);
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRole(Role role)
        {
            if (ModelState.IsValid)
            {
                _role.AddRole(role);
            }
            else
            {
                ModelState.AddModelError("", "Wrong Detail Add.");
            }
            return View("Index");
        }



        public IActionResult EditRole(int uId)
        {
            
            var user = _user.GetUserByID(uId).Email;
            ViewBag.Email = user.ToString();

            TempData["uId"] = uId;
                return View(_role.GetAllRole(uId));
        }
        [HttpPost]
        public IActionResult EditRole(List<RoleEdit> RoleEdit)
        {
            int UId = (int)TempData["uId"];
            var roleChk = RoleEdit.Where(x => x.Checked == true);
            
            foreach (var item in roleChk)
            {
                
                if(item.Checked = true)
                {
                    _role.AddUserRole(UId, item.RId);
                }
            };

            var roleUchk = RoleEdit.Where(x => x.Checked == false);
            foreach (var item in roleUchk)
            {
                    _role.RemoveRole(UId, item.RId);
                
            };
            return RedirectToAction("Index");
        }

    }
}
