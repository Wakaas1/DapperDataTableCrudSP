using Dapper;
using DapperStoredProc.Models;
using DapperStoredProc.Models.DataTable;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Linq;
namespace DapperStoredProc.Data
{

    public class DapperRepo : IDapperRepo
    {
        private string connectionString;

        public List<EmployeePartial> EmployeePartial { get; private set; }
        public IEnumerable<int> Total { get; private set; }

        public DapperRepo(IConfiguration configuration)
        {
            connectionString = configuration.GetConnectionString("ConnGCU");
        }


        public T ExecuteReturnScalar<T>(string procrdureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                return (T)Convert.ChangeType(sqlCon.Execute(procrdureName, param, commandType: CommandType.StoredProcedure), typeof(T));
            }

        }
        public void Execute(string procrdureName, DynamicParameters param )
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                 sqlCon.Execute(procrdureName, param, commandType: CommandType.StoredProcedure);
            }

        }

        //DapperORM.RetrunList<EmployeeModel> <= IEnumberable<EmployeeModel>

        public IEnumerable<T> ReturnList<T>(string procrdureName, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                return sqlCon.Query<T>(procrdureName, param, commandType: CommandType.StoredProcedure);
            }

        }
        public int CreateEmployeeReturnInt(string StoredProcedure, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(StoredProcedure, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("EmpId");
            }
        }
       
        public int CreateUserReturnInt(string StoredProcedure, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(StoredProcedure, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("Id");
            }
        }

        public int CreateUserReturnFKInt(string StoredProcedure, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(StoredProcedure, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("UserId");
            }
        }

        public int CreateUserReturnRoleInt(string StoredProcedure, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(StoredProcedure, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("RId");
            }
        }

        public int CreateUserReturnSubjectInt(string StoredProcedure, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                sqlCon.Execute(StoredProcedure, param, commandType: CommandType.StoredProcedure);
                return param.Get<int>("subId");
            }
        }
        public async Task<DataTableResponse<T>> ReturnListMultiple<T>(string procrdureName, DynamicParameters param = null)
        {
            var list = new List<T>(); int total = 0; 
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                using (var query = await sqlCon.QueryMultipleAsync(procrdureName, param, commandType: CommandType.StoredProcedure))
                {
                   
                    list = query.Read<T>().AsList<T>();                    
                    if (!query.IsConsumed)
                        total = query.Read<int>().FirstOrDefault();
                    
                }
            }
            return new DataTableResponse<T>()
            {
                
                data = list,
                recordsFiltered = total,
                recordsTotal = total
            };
        }

        //public object ReturnListMultiple(string procrdureName, DynamicParameters param = null)
        //{

        //    using (SqlConnection sqlCon = new SqlConnection(connectionString))
        //    {
        //        sqlCon.Open();
        //        using (var query = sqlCon.QueryMultiple(procrdureName, param, commandType: CommandType.StoredProcedure))
        //        {
        //            var dt = new DataResponse();
        //            dt.Record = query.Read<Employee>().AsList<Employee>();
        //            if (!query.IsConsumed)
        //                dt.TotalRec = query.Read<int>();
        //            return dt;
        //        }
        //    }

        //}


    }
}

