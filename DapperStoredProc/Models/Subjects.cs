using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models
{
    public class Subjects
    {
        [Key]
        public int subId { get; set; }
        public string SubjectName { get; set; }
    }
}
