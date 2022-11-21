using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models.DataTable
{
   
        public class DataTableResponse<T>
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public List<T> data { get; set; }
            public string error { get; set; }
        }
    
}
