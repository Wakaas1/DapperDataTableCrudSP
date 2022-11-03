using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.DTO
{
    public partial class UserRolePartial
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Image { get; set; }

        public string Token { get; set; }
        public string IsVerify { get; set; }

     
    }
    public partial class UserRolePartial
    {
        public int UserId { get; set; }
        public int RoleId { get; set; }

    }
    public partial class UserRolePartial
    {
        public int RId { get; set; }
        public string RName { get; set; }
    }


    
}
