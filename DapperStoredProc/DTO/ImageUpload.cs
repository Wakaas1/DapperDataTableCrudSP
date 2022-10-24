using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.DTO
{
    public class ImageUpload
    {
        [Key]
        public int id { get; set; }
        public string postedFile { get; set; }

        [NotMapped]
        [FileExtension]
        public IFormFile UploadImage { get; set; }
    }
}
