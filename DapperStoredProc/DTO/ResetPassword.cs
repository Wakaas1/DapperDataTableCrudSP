using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.DTO
{
    public class ResetPassword
    {
      

        [Required]
        public string Token { get; set; } 
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirma new password do not match.")]
        public string ConfirmNewPassword { get; set; }

        //[Display(Name = "Return Token")]
        //public string ReturnToken { get; set; }
    }
}
