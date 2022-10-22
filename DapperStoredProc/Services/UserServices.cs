using Dapper;
using DapperStoredProc.Data;
using DapperStoredProc.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Services
{
    public class UserServices : IUserServices
    {
        //private readonly IConfiguration _configuration;
        private readonly IDapperRepo _dapperRepo;

        public UserServices(IConfiguration configuration, IDapperRepo dapperRepo)
        {
            //_configuration = configuration;
            //connectionString = _configuration.GetConnectionString("ConnGCU");
            //providerName = "System.Data.SqlClient";
            _dapperRepo = dapperRepo;
        }

        //public string connectionString { get; }
        //public string providerName { get; }

        public int AddUser(User model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@Name", model.Name);
            param.Add("@Email", model.Email);
            param.Add("@Password", model.Password);
            var result = _dapperRepo.CreateUserReturnInt("dbo.AddUser", param);
            if (result > 0)
            {

            }

            return result;
        }

        public User GetEmpByEmail(string model)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Email", model);
            var user = _dapperRepo.ReturnList<User>("dbo.GetUserByEmail", param).FirstOrDefault();

            return user;
        }
        public void CheckPassword(User model)
        {
            
        }
     
    }
}
