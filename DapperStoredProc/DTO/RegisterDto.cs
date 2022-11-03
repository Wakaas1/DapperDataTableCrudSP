using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.DTO
{
    public class RegisterDto
    {
    
        //[Required(AllowEmptyStrings = false, ErrorMessage = "User Name is requierd")]
        public string Name { get; set; }

        //[Remote("IsUserAlreadyExists", "User" , ErrorMessage = "Email Already Exist")]
        //[Required(AllowEmptyStrings = false, ErrorMessage = "Email is requierd")]
        public string Email { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Password is requierd")]
        //[DataType(DataType.Password)]
        //[MinLength(6, ErrorMessage = "Need min 6 character")]
        public string Password { get; set; }

        //[Required(AllowEmptyStrings = false, ErrorMessage = "Confirm Password is requierd")]
        //[DataType(DataType.Password)]
        //[Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
        


    }
}
