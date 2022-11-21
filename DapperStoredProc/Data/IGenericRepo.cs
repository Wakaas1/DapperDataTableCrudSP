using DapperStoredProc.Models.DataTable;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperStoredProc.Data
{
    public interface IGenericRepo
    {
        Task<List<EmployeePartial>> GetEmployeeAsync(ListingRequest request);
        Task<List<UserPartial>> GetUserAsync(ListingRequest request);
        Task<List<EmployeePartial>> GetAllEmployee(DTReq request);
        Task<DataTableResponse<EmployeePartial>> GetAllEmployeeMultiple(DTReq request);
        Task<DataTableResponse<UserPartial>> GetAllUserMultiple(DTReq request);
    }
}