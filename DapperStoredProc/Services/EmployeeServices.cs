using Dapper;
using DapperStoredProc.Data;
using DapperStoredProc.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Services
{
    public class EmployeeServices : IEmployeeServices
    {
        private readonly IConfiguration _configuration;
        private readonly IDapperRepo _dapperRepo;

        public EmployeeServices(IConfiguration configuration, IDapperRepo dapperRepo)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("ConnGCU");
            providerName = "System.Data.SqlClient";
            _dapperRepo = dapperRepo;
        }
        public string connectionString { get; }
        public string providerName { get; }

      
        public IEnumerable<Employee> GetAllEmployees()
        {
            List<Employee> empList = new List<Employee>();
            empList = _dapperRepo.ReturnList<Employee>("GetAllEmp").ToList();
            return (empList);
        }
        

        public Employee GetEmpByID(int EmpId)
        {
            
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", EmpId);
            var emp = _dapperRepo.ReturnList<Employee>("dbo.GetEmployeeByID", param).FirstOrDefault();

            return emp;
        }

        public int AddEmployee(Employee model)
        {           
                Dapper.DynamicParameters param = new DynamicParameters();
                param.Add("@EmpId", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
                param.Add("@EmployeeName", model.EmployeeName);
                param.Add("@Department", model.Department);
                param.Add("@Designation", model.Designation);
                var result = _dapperRepo.CreateReturnInt("dbo.AddEmployee", param);
                if (result > 0)
                {

                }
           
            return result;
        }


        public int UpdateEmployee(Employee model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", model.EmpId);
            param.Add("@EmployeeName", model.EmployeeName);
            param.Add("@Department", model.Department);
            param.Add("@Designation", model.Designation);
            var result = _dapperRepo.CreateReturnInt("dbo.UpdateEmployee", param);
            

            return result;
        }
      
        public int DeleteEmployee(int EmpId)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", EmpId);
            var emp = _dapperRepo.CreateReturnInt("dbo.DeleteEmployee", param);

            return emp;
        }
    }
}
