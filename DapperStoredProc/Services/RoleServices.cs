using Dapper;
using DapperStoredProc.Data;
using DapperStoredProc.DTO;
using DapperStoredProc.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Services
{
    public class RoleServices : IRoleServices
    {
        private readonly IDapperRepo _dapperRepo;

        public RoleServices(IDapperRepo dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }


        public int AddRole(Role model)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@RId", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@RName", model.RName);
            return _dapperRepo.CreateUserReturnRoleInt("AddRole", param);
        }


        public int AddUserRole(int userId, int roleId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@Id", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@UserId", userId);
            param.Add("@RoleId", roleId);
            return _dapperRepo.CreateUserReturnFKInt("AddUserRole", param);
        }
        public List<RoleEdit> GetAllRole(int uId)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@userid", uId);
            return _dapperRepo.ReturnList<RoleEdit>("dbo.GetAllRole", param).ToList();
            
        }
      
        public void RemoveRole(int userId, int roleId)
        {
            DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", userId);
            param.Add("@RoleId", roleId);
            _dapperRepo.CreateUserReturnFKInt("DeleteRole", param);
        }
        public IEnumerable<UserDetail> GetAllUsers(UserDetail model)
        {
            List<UserDetail> user = new List<UserDetail>();
            user = _dapperRepo.ReturnList<UserDetail>("GetUserByRole").ToList();
            return (user);
        }

        //public int AddRole(Role model)
        //{
        //    Dapper.DynamicParameters param = new DynamicParameters();
        //    param.Add("@RId", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
        //    param.Add("@RName", model.RName);

        //    var result = _dapperRepo.CreateUserReturnInt("dbo.AddRole", param);
        //    if (result > 0)
        //    { }
        //    return result;
        //}
        //public int UpdateRole(Role model)
        //{
        //    Dapper.DynamicParameters param = new DynamicParameters();
        //    param.Add("@RId", model.RId);
        //    param.Add("@RName", model.RName);

        //    var result = _dapperRepo.CreateUserReturnInt("dbo.UpdateRole", param);
        //    if (result > 0)
        //    { }
        //    return result;
        //}

        //public Role GetRoleById(int Id)
        //{

        //    Dapper.DynamicParameters param = new DynamicParameters();
        //    param.Add("@RId", Id);
        //    var user = _dapperRepo.ReturnList<Role>("dbo.GetUserByRoleId", param).FirstOrDefault();

        //    return user;
        //}

        //public int DeleteRole(int Id)
        //{
        //    Dapper.DynamicParameters param = new DynamicParameters();
        //    param.Add("@RId", Id);
        //    var user = _dapperRepo.CreateEmployeeReturnInt("dbo.DeleteRole", param);

        //    return user;
        //}
    }
}
