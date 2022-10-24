using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Dapper;
using DapperStoredProc.Data;
using DapperStoredProc.Models;
using DapperStoredProc.Models.DataTable;
using DapperStoredProc.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace DapperStoredProc.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly IEmployeeServices _services;
       

        public EmployeeController(IEmployeeServices services)
        {
            _services = services;
            
            
        }
        public IActionResult Index()
        {
            return View(_services.GetAllEmployees());
        }
      public IActionResult GetEmpByID(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            var emp = _services.GetEmpByID(id.GetValueOrDefault());
            if (emp == null)

                return NotFound();

            return View(emp);
        }

        public IActionResult AddEmployee()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddEmployee([Bind("EmployeeName,Department,Designation")]Employee employee)
        {
            long result = 0;
            int Status;
            string Value;
           // ModelState.Remove("EmpId");
            if (ModelState.IsValid)
            {
                result = _services.AddEmployee(employee);
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
        [HttpGet]
        public IActionResult UpdateEmployee(int? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var emp = _services.GetEmpByID(id.GetValueOrDefault());
            if (emp == null)
        
                return NotFound();
            
            return View(emp);
        }

        [HttpPost]
        public IActionResult UpdateEmployee(int id, [Bind("EmpId,EmployeeName,Department,Designation")] Employee employee)
        {
            long result = 0;
            int Status;
            string Value;
            //ModelState.Remove("EmpId");
            if (ModelState.IsValid)
            {
               result = _services.UpdateEmployee(employee);
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
        [HttpGet]
        public IActionResult DeleteEmployee(int? id)
        {
           
            if (id == null)
            {
                return NotFound();
            }
            _services.DeleteEmployee(id.GetValueOrDefault());
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult DeleteEmployee(int id, Employee employee)
        {
            if (_services.DeleteEmployee(id)>0)
            {
                return RedirectToAction("Index");
            }
            return View(employee);
        }
        //[HttpPost]
        //public IActionResult DataTable()
        //{
        //    int totalRecord = 0;
        //    int filterRecord = 0;

        //    var draw = Request.Form["draw"].FirstOrDefault();

        //    // Sort Column Name
        //    var sortCoulmn = Request.Form["sortColumn[" + Request.Form["order[0] [column]"].FirstOrDefault() + "][name]"].FirstOrDefault();

        //    var sortColumnDirection = Request.Form["order[0][dir]"].FirstOrDefault();

        //        //Search value from (Search box)
        //    var searchValue = Request.Form["search[value]"].FirstOrDefault();

        //    //Paging Size (10,20,50,100)
        //    int pageSize = Convert.ToInt32(Request.Form["length"].FirstOrDefault() ?? "0");

        //    // Skip Number of Count
        //    int skip = Convert.ToInt32(Request.Form["start"].FirstOrDefault() ?? "0");
            
        //    //getting all Employee data 
        //    var data = _context.Set<Employee>().AsQueryable();

        //    //get total count of data in table
        //    totalRecord = data.Count();

        //    // search data when search value found
        //    if(!string.IsNullOrEmpty(searchValue))
        //    {
        //        data = data.Where(a => a.EmployeeName.ToLower().Contains(searchValue.ToLower()) || a.Department.ToLower().Contains(searchValue.ToLower()) || a.Designation.ToLower().Contains(searchValue.ToLower())); 
        //    }

        //    //get total count of records after search 
        //    filterRecord = data.Count();

        //    //sort data
        //    if (!string.IsNullOrEmpty(sortCoulmn) && !string.IsNullOrEmpty(sortColumnDirection)) data = data.OrderBy(sortCoulmn + " " + sortColumnDirection);

        //    //pagination
        //    var empList = data.Skip(skip).Take(pageSize).ToList();
        //    return Json(new
        //    {
        //        draw = draw,
        //        recordsTotal = totalRecord,
        //        recordsFiltered = filterRecord,
        //        data = empList
        //    });
        //}
        [HttpPost]
        public JsonResult GetAllEmployee()
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
            return Json(_services.GetAllEmployeeAsync(request).Result);
        }

    }
}