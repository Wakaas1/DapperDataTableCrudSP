using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Models.DataTable
{
    public class EmployeeMultiple
    {
        public int EmpId { get; set; }
        public string EmployeeName { get; set; }

        public string Designation { get; set; }

        public string Department { get; set; }

    }
    public class Total
    {
        public int TotalCount { get; set; }
        public int FilteredCount { get; set; }

    }
    public class DataResponse
    {
        public IEnumerable<Employee> Record { get; set; }
        public int TotalRec { get; set; }
    }
}


