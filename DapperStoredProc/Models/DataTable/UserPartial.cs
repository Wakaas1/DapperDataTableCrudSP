using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models.DataTable
{
    public partial class UserPartial
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }        
        public string roles_list { get; set; }
      
    }
    public partial class UserPartial
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }


    }
}
