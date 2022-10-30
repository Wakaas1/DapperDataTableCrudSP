using Dapper;
using DapperStoredProc.Data;
using DapperStoredProc.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace DapperStoredProc.Services
{
    public class UserServices : IUserServices
    {
        
        private readonly IDapperRepo _dapperRepo;

        public UserServices(IDapperRepo dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        public int AddUser(Users model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@Name", model.Name);
            param.Add("@Email", model.Email);
            param.Add("@Password", model.Password);
            param.Add("@Image", model.Image);
            param.Add("@Role", model.Role);
            param.Add("@Token", model.Token);
            var result = _dapperRepo.CreateUserReturnInt("dbo.AddUser", param);
            if (result > 0)
            {

            }

            return result;
        }
        public int UpdateUser(Users model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", model.id);
            param.Add("@Name", model.Name);
            param.Add("@Email", model.Email);
            param.Add("@Password", model.Password);
            param.Add("@Image", model.Image);
            param.Add("@Role", model.Role);
            param.Add("@Token", model.Token);
            var result = _dapperRepo.CreateUserReturnInt("dbo.UpdateUser", param);
            if (result > 0)
            {

            }

            return result;
        }

        public Users GetUserByEmail(string model)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Email", model);
            var user = _dapperRepo.ReturnList<Users>("dbo.GetUserByEmail", param).FirstOrDefault();

            return user;
        }

        public int UpadateUserImage(Users model)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", model.id);
            param.Add("@Image", model.Image);
            var result = _dapperRepo.CreateUserReturnInt("dbo.UserUpdateImage", param);
            if (result > 0)
            {

            }

            return result;
        }
        public string CreatePasswordHash(string password)
        {

            var hmac = new HMACSHA512();

            byte[] passwordSalt = passwordSalt = hmac.Key;
            byte[] passwordHash = passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            string Passalt = Convert.ToBase64String(passwordSalt);
            string Pashash = Convert.ToBase64String(passwordHash);

            var createHash = Pashash + ":"+ Passalt;
            return createHash;
        }

        public bool VerifyPasswordHash(string dbpassword, string password)
        {
            string[] passwordarry = dbpassword.Split(':');
            byte[] orignalhash = Convert.FromBase64String(passwordarry[1]);
            using (var hmac = new HMACSHA512(orignalhash))
            {
                var verifyHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                var orignalsalt = Convert.FromBase64String(passwordarry[0]);
                return verifyHash.SequenceEqual(orignalsalt);
            }
        }

        public void UpdatePassword(string email, string password)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            
       
            param.Add("@Email", email);
            param.Add("@Password", password);
             _dapperRepo.Execute("dbo.UpdatePassword", param);
          
        }

        public void UpdateToken(string email, string token)
        {
            Dapper.DynamicParameters param = new DynamicParameters();


            param.Add("@Email", email);
            param.Add("@Token", token);
             _dapperRepo.Execute("dbo.UpdateToken", param);

        }
    }
}
