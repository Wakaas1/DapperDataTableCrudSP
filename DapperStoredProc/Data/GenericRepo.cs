using Dapper;
using DapperStoredProc.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Data
{


    public class GenericRepo : IGenericRepo
    {
        private readonly IDapperRepo _dapperRepo;
        public GenericRepo(IDapperRepo dapperRepo)
        {
            _dapperRepo = dapperRepo;
        }

        

        public async Task<List<EmployeePartial>> GetEmployeeAsync(ListingRequest request)
        {
            try
            {
                Dapper.DynamicParameters param = new DynamicParameters();
                param.Add("SearchValue", request.SearchValue, DbType.String);
                param.Add("PageNo", request.PageNo, DbType.Int32);
                param.Add("PageSize", request.PageSize, DbType.Int32);
                param.Add("SortColumn", request.SortColumn, DbType.Int32);
                param.Add("SortDirection", request.SortDirection, DbType.String);

                return _dapperRepo.ReturnList<EmployeePartial>("GetAllEmpSP1", param).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<UserPartial>> GetUserAsync(ListingRequest request)
        {
            try
            {
                Dapper.DynamicParameters param = new DynamicParameters();
                param.Add("SearchValue", request.SearchValue, DbType.String);
                param.Add("PageNo", request.PageNo, DbType.Int32);
                param.Add("PageSize", request.PageSize, DbType.Int32);
                param.Add("SortColumn", request.SortColumn, DbType.Int32);
                param.Add("SortDirection", request.SortDirection, DbType.String);

                return _dapperRepo.ReturnList<UserPartial>("GetAllUserSP", param).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<EmployeePartial>> GetAllEmployee(DTReq request)
        {
            try
            {
                Dapper.DynamicParameters param = new DynamicParameters();
                param.Add("SearchText", request.SearchText, DbType.String);
                param.Add("SortExpression", request.SortExpression, DbType.String);
                param.Add("StartRowIndex", request.StartRowIndex, DbType.Int32);
                param.Add("PageSize", request.PageSize, DbType.Int32);
                

                return _dapperRepo.ReturnList<EmployeePartial>("GetAllEmpDT", param).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<DataTableResponse<EmployeePartial>> GetAllEmployeeMultiple(DTReq request)
        {
            try
            {
                Dapper.DynamicParameters param = new DynamicParameters();
                param.Add("SearchText", request.SearchText, DbType.String);
                param.Add("SortExpression", request.SortExpression, DbType.String);
                param.Add("StartRowIndex", request.StartRowIndex, DbType.Int32);
                param.Add("PageSize", request.PageSize, DbType.Int32);

                return await _dapperRepo.ReturnListMultiple<EmployeePartial>("GetAllEmpDT", param);
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<DataTableResponse<UserPartial>> GetAllUserMultiple(DTReq request)
        {
            try
            {
                Dapper.DynamicParameters param = new DynamicParameters();
                param.Add("SearchText", request.SearchText, DbType.String);
                param.Add("SortExpression", request.SortExpression, DbType.String);
                param.Add("StartRowIndex", request.StartRowIndex, DbType.Int32);
                param.Add("PageSize", request.PageSize, DbType.Int32);

                return await _dapperRepo.ReturnListMultiple<UserPartial>("GetAllUserDT", param);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }



    }
}
