using DapperStoredProc.Models;
using System.Collections.Generic;

namespace DapperStoredProc.Services
{
    public interface IEmployeeServices
    {
        public IEnumerable<Employee> GetAllEmployees();
        public Employee GetEmpByID(int EmpId);
        public int AddEmployee(Employee model);
        public int UpdateEmployee(Employee model);
        
        public int DeleteEmployee(int EmpId);
    }
}