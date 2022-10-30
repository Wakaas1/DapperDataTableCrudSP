using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DapperStoredProc.Data
{

    public class DapperRepo : IDapperRepo
    {
        private string connectionString;

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
        public int Delete(string StoredProcedure, DynamicParameters param = null)
        {
            using (SqlConnection sqlCon = new SqlConnection(connectionString))
            {
                sqlCon.Open();
                return sqlCon.Execute(StoredProcedure, param, commandType: CommandType.StoredProcedure);
               
            }
        }
    }
}

