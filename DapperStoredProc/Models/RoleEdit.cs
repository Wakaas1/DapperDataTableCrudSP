using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models
{
    public class RoleEdit
    {
        
        public string Email { get; set; }
        public IEnumerable<Role> Role { get; set; }
        public string TakenRole { get; set; }
        
    }
}
