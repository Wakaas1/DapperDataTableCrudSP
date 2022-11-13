﻿using Dapper;
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
                var parameters = new DynamicParameters();
                parameters.Add("SearchValue", request.SearchValue, DbType.String);
                parameters.Add("PageNo", request.PageNo, DbType.Int32);
                parameters.Add("PageSize", request.PageSize, DbType.Int32);
                parameters.Add("SortColumn", request.SortColumn, DbType.Int32);
                parameters.Add("SortDirection", request.SortDirection, DbType.String);
                return _dapperRepo.ReturnList<EmployeePartial>("GetAllEmpSP1", parameters).ToList();
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
                var parameters = new DynamicParameters();
                parameters.Add("SearchValue", request.SearchValue, DbType.String);
                parameters.Add("PageNo", request.PageNo, DbType.Int32);
                parameters.Add("PageSize", request.PageSize, DbType.Int32);
                parameters.Add("SortColumn", request.SortColumn, DbType.Int32);
                parameters.Add("SortDirection", request.SortDirection, DbType.String);
                return _dapperRepo.ReturnList<UserPartial>("GetAllUserSP", parameters).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        public async Task<List<EmployeePartial>> GetAllEmployee(ListingRequestDT request)
        {
            try
            {
                var parameters = new DynamicParameters();
                parameters.Add("SearchText", request.SearchText, DbType.String);
                parameters.Add("SortExpression", request.SortExpression, DbType.String);
                parameters.Add("StartRowIndex", request.StartRowIndex, DbType.Int32);
                parameters.Add("PageSize", request.PageSize, DbType.Int32);
                
                return _dapperRepo.ReturnList<EmployeePartial>("GetAllEmpDT", parameters).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }







    }
}
