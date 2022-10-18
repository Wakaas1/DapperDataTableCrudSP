using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models
{
    public class Employee
    {
        [Key]
        

        public int EmpId { get; set; }
        [Required(ErrorMessage = "Name of Employee is Required.")][Display(Name ="Employee Name")]
        public string EmployeeName { get; set; }
        [Required(ErrorMessage = "Name of Designation is Required.")]
        public string Designation { get; set; }
        [Required(ErrorMessage = "Name of Department is Required.")]
        public string Department { get; set; }
       
        //public string Response { get; set; }
    }
}
