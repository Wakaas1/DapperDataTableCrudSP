using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models.DataTable
{
    public partial class EmployeePartial
    {
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }
        
        public string Designation { get; set; }
        
        public string Department { get; set; }
        public string SubjectName { get; set; }

    }
    public partial class EmployeePartial
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }


    }


}
