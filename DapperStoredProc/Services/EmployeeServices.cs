using Dapper;
using DapperStoredProc.Data;
using DapperStoredProc.Models;
using DapperStoredProc.Models.DataTable;
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
      
        private readonly IDapperRepo _dapperRepo;
        private readonly IGenericRepo _genericRepo;

        public EmployeeServices( IDapperRepo dapperRepo, IGenericRepo genericRepo)
        {
          
            _dapperRepo = dapperRepo;
            _genericRepo = genericRepo;
        }
 

      
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
                var result = _dapperRepo.CreateEmployeeReturnInt("dbo.AddEmployee", param);
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
            var result = _dapperRepo.CreateEmployeeReturnInt("dbo.UpdateEmployee", param);
            

            return result;
        }
      
        public int DeleteEmployee(int EmpId)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@EmpId", EmpId);
            var emp = _dapperRepo.CreateEmployeeReturnInt("dbo.DeleteEmployee", param);

            return emp;
        }
        public async Task<DataTableResponse<EmployeePartial>>GetAllEmployeeAsync(DataTableRequest request)
        {
            var req = new ListingRequest()
            {
                PageNo = request.Start,
                PageSize = request.Length,
                SortColumn = request.Order[0].Column,
                SortDirection = request.Order[0].Dir,
                SearchValue = request.Search != null ? request.Search.Value.Trim() : ""
            };
            var employee = await _genericRepo.GetEmployeeAsync(req);
            return new DataTableResponse<EmployeePartial>()
            {
                Draw = request.Draw,
                RecordsTotal = employee[0].TotalCount,
                RecordsFiltered = employee[0].FilteredCount,
                Data = employee.ToArray(),
                Error = ""
            };
        }

    }
}
