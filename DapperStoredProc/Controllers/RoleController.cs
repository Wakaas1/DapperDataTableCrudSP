using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        //[HttpGet]
        //public IActionResult CreateRole()
        //{
        //    return View();
        //}

        //[HttpPost]
        //public IActionResult CreateRole(Role role )
        //{
        //    if(ModelState.IsValid)
        //    {
        //        _role.AddRole(role);
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Wrong Detail Add.");
        //    }
        //    return View("Index");
        //}

        //[HttpGet]
        //public IActionResult EditRole(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }
        //    var emp = _role.GetRoleById(id.GetValueOrDefault());
        //    if (emp == null)

        //        return NotFound();

        //    return View(emp);
        //}

        //[HttpPost]
        //public IActionResult EditRole(int id, [Bind("RId,RName")] Role role)
        //{
        //    long result = 0;
        //    int Status;
        //    string Value;

        //    if (ModelState.IsValid)
        //    {
        //        result = _role.UpdateRole(role);
        //        if (result > 0)
        //        {
        //            Status = 200;
        //            Value = Url.Content("~/Design/View/");
        //        }
        //        else
        //        {
        //            Status = 500;
        //            Value = "There is some error at server side";
        //        }
        //    }
        //    else
        //    {
        //        Status = 500;
        //        Value = "There is some error at client side";
        //    }
        //    return Json(new { status = Status, value = Value });
        //}


        public IActionResult EditRole(int uId)
        {
            var user = _user.UserListId(uId).ToArray();
             var roles = _role.GetAllRole();
             
            foreach(var item in user)
            {
                var rolelist = item.RName.ToList();
            }
            var role = new RoleEdit
                {
                    Email = user[0].Email,
                    Role = roles.ToList(),
                    TakenRole = user[0].RName
                };
            TempData["uId"] = uId;
            return View(role);
        }
        [HttpPost]
        public IActionResult EditRole(int uId, int rID)
        {
            int Uid = (int)TempData["uId"];
            if (rID == 0)
            {
                _role.AddRole(Uid, rID);
            }
            else
            {
                _role.RemoveRole(Uid, rID);
            }
            return View();
        }

    }
}
