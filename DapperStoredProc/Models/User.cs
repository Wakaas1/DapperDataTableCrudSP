﻿using DapperStoredProc.Controllers;
using DapperStoredProc.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models
{
    public class User
    {
       [Key]
       public int id { get; set; }
       public string Name { get; set; }
       public string Email { get; set; }
       public string Password { get; set; }
       public string Image { get; set; }


        [NotMapped]
        [FileExtension]
        public IFormFile UploadImage { get; set; }

        //public HttpPostedFileBase fileBase  { get; set; }
    }
}
