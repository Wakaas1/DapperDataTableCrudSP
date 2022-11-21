using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models.DataTable
{
    public class ListingRequestDT
    {
        public string SearchText { get; set; }
        public string SortExpression { get; set; } = "ASC";
        public int StartRowIndex { get; set; } 
        public int PageSize { get; set; } 
        
    }
}
