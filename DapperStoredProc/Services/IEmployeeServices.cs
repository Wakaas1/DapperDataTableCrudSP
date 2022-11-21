using DapperStoredProc.Models;
using DapperStoredProc.Models.DataTable;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperStoredProc.Services
{
    public interface IEmployeeServices
    {
        public IEnumerable<Employee> GetAllEmployees();
        public Employee GetEmpByID(int EmpId);
        public int AddEmployee(Employee model);
        public int UpdateEmployee(Employee model);
        //Task<DataTableResponse<EmployeePartial>> GetAllEmployeeAsync(DataTableRequest request);
        public int DeleteEmployee(int EmpId);
        Task<DataTableResponse<EmployeePartial>> GetAllEmployeeDT(DataTableRequest request);
        //Task<DataTableResponse<Employee>> GetAllEmployeeDTMultiReq(DTReq request);
    }
}