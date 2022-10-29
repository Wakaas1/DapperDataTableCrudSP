using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models
{
    public class EmailCredential
    {
        

        public string UserName { get; set; }
        public string Password { get; set; }
        public string fromMail { get; internal set; }
        public string fromMailPassword;
    }
}
