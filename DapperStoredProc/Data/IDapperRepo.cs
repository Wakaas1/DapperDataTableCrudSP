using Dapper;
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
        int CreateReturnInt(string StoredProcedure, DynamicParameters param = null);
        int Delete(string StoredProcedure, DynamicParameters param = null);
    }
}
