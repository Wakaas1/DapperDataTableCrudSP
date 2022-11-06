using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models
{
    public class UserDetail
    {
      public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string roles_list { get; set; }
    }
}
