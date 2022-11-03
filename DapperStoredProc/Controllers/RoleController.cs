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
        public RoleController(IRoleServices role)
        {
            _role = role;
        }
        public IActionResult Index()
        {
            return View(_role.GetAllRole());
        }

        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateRole(Role role )
        {
            if(ModelState.IsValid)
            {
                _role.AddRole(role);
            }
            else
            {
                ModelState.AddModelError("", "Wrong Detail Add.");
            }
            return View();
        }

        [HttpGet]
        public IActionResult DeleteRole(int id)
        {
            if (id == null)
            {
                return NotFound();
            }
            _role.DeleteRole(id);
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteRole(int id, Role role)
        {
            if (_role.DeleteRole(id) > 0)
            {
                return RedirectToAction("Index");
            }
            return View(role);
        }
    }
}
