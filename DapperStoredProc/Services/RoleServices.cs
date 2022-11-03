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
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@RId", -1, dbType: DbType.Int32, direction: ParameterDirection.Output);
            param.Add("@RName", model.RName);

            var result = _dapperRepo.CreateUserReturnInt("dbo.AddRole", param);
            if (result > 0)
            { }
            return result;
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

        public Role GetRoleById(int Id)
        {

            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@RId", Id);
            var user = _dapperRepo.ReturnList<Role>("dbo.GetRoleById", param).FirstOrDefault();

            return user;
        }

        public int DeleteRole(int Id)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@RId", Id);
            var user = _dapperRepo.CreateEmployeeReturnInt("dbo.DeleteRole", param);

            return user;
        }

        public IEnumerable<Role> GetAllRole()
        {
            List<Role> role = new List<Role>();
            role = _dapperRepo.ReturnList<Role>("GetUserByRole").ToList();
            return (role);
        }

        public IEnumerable<UserRolePartial> UserList(int id)
        {
            Dapper.DynamicParameters param = new DynamicParameters();
            param.Add("@UserId", id);
          return _dapperRepo.ReturnList<UserRolePartial>("dbo.GetRoleByUserId", param);
        }


        //public async Task<UserRoleListReq<UserRolePartial>> UserList(int id)
        //{
        //   new 
        //}
    }
}
