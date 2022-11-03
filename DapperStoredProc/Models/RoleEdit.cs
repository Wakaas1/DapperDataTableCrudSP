using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models
{
    public class RoleEdit
    {
        public string Role { get; set; }
        public IEnumerable<Users> Members { get; set; }
        public IEnumerable<Users> NonMembers { get; set; }
        public string RoleName { get; set; }
        public string[] AddIds { get; set; }
        public string[] DeleteIds { get; set; }
    }
}
