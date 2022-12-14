using Dapper;
using DapperStoredProc.Models.DataTable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperStoredProc.Data
{
    public interface IDapperRepo
    {
       T ExecuteReturnScalar<T>(string procrdureName, DynamicParameters param = null);
        IEnumerable<T> ReturnList<T>(string procrdureName, DynamicParameters param = null);
        int CreateEmployeeReturnInt(string StoredProcedure, DynamicParameters param = null);
        int CreateUserReturnInt(string StoredProcedure, DynamicParameters param = null);
        void Execute(string procrdureName, DynamicParameters param);
        int CreateUserReturnFKInt(string StoredProcedure, DynamicParameters param = null);
        int CreateUserReturnRoleInt(string StoredProcedure, DynamicParameters param = null);
        Task<DataTableResponse<T>> ReturnListMultiple<T>(string procrdureName, DynamicParameters param = null);
        //Task<DataResponse> ReturnMultiple(string procrdureName, DynamicParameters param = null);
        int CreateUserReturnSubjectInt(string StoredProcedure, DynamicParameters param = null);
    }
}
