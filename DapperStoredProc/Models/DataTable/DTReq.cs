using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models.DataTable
{
    public class DTReq
    {
        public string SearchText { get; set; }
        public string SortExpression { get; set; }
        public int StartRowIndex { get; set; }
        public int PageSize { get; set; }
       // public string Department { get; set; }
        public int SubjectId { get; set; }
         
        
    }
}
