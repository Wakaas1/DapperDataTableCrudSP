using Dapper;
using DapperStoredProc.Data;
using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using DapperStoredProc.Models.DataTable;
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
        private readonly IGenericRepo _genericRepo;

        public UserServices(IDapperRepo dapperRepo, IGenericRepo genericRepo)
        {
            _dapperRepo = dapperRepo;
            _genericRepo = genericRepo;
        }

        public int AddUser(Users model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@Name", model.Name);
            param.Add("@Email", model.Email);
            param.Add("@Password", model.Password);
            param.Add("@Image", model.Image);            
            param.Add("@Token", model.Token);
            param.Add("@IsVerify", model.IsVerify);
            param.Add("@TokenGeneratedDate", model.TokenGeneratedDate);
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
            param.Add("@Token", model.Token);
            var result = _dapperRepo.CreateUserReturnInt("dbo.UpdateUser", param);
            if (result > 0)
            {

            }

            return result;
        }
        public Users GetUserByID(int Id)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id);
            var user = _dapperRepo.ReturnList<Users>("dbo.GetUserByID", param).FirstOrDefault();

            return user;
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

        public int DeleteUser(int Id)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@Id", Id);
            var user = _dapperRepo.CreateEmployeeReturnInt("dbo.DeleteUser", param);

            return user;
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

        public IEnumerable<UserRolePartial> UserListId(int id)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", id);
            return _dapperRepo.ReturnList<UserRolePartial>("dbo.GetUserByRoleId", param);
        }


        public IEnumerable<UserDetail> GetAllUsers(UserDetail model)
        {
            List<UserDetail> user = new List<UserDetail>();
            user = _dapperRepo.ReturnList<UserDetail>("GetUserByRole").ToList();
            return (user);
        }

        public IEnumerable<Role> GetAllRole(Role model)
        {
            List<Role> role = new List<Role>();
            role = _dapperRepo.ReturnList<Role>("GetAllRole").ToList();
            return (role);
        }
        public void UserIsVerified(string email, bool verify)
        {
            Dapper.DynamicParameters param = new DynamicParameters();


            param.Add("@Email", email);
            param.Add("@IsVerify", verify);
            _dapperRepo.Execute("dbo.IsAVirify", param);

        }

        // User Roles Create/Update

        public int AddRole(int userId, int roleId)
        {
            DynamicParameters parameters = new DynamicParameters();
            parameters.Add("@UserId", userId);
            parameters.Add("@RoleId", roleId);
            return _dapperRepo.CreateUserReturnInt("AddUserRole", parameters);
        }

        public int UpdateRole(Role model)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@RId", model.RId);
            param.Add("@RName", model.RName);

            var result = _dapperRepo.CreateUserReturnInt("dbo.UpdateRole", param);
            if (result > 0)
            { }
            return result;
        }
        public Role GetRoleById(int id)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", id);
            var user = _dapperRepo.ReturnList<Role>("dbo.GetUserByRoleId", param).FirstOrDefault();

            return user;
        }

        public async Task<DataTableResponse<UserPartial>> GetAllUserAsync(DataTableRequest request)
        {
            var req = new ListingRequest()
            {
                PageNo = request.Start,
                PageSize = request.Length,
                SortColumn = request.Order[0].Column,
                SortDirection = request.Order[0].Dir,
                SearchValue = request.Search != null ? request.Search.Value.Trim() : ""
            };
            var users = await _genericRepo.GetUserAsync(req);
            return new DataTableResponse<UserPartial>()
            {
                Draw = request.Draw,
                RecordsTotal = users[0].TotalCount,
                RecordsFiltered = users[0].FilteredCount,
                Data = users.ToArray(),
                Error = ""
            };
        }
    }
}
