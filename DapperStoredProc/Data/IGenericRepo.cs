using DapperStoredProc.Models.DataTable;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperStoredProc.Data
{
    public interface IGenericRepo
    {
        Task<List<EmployeePartial>> GetEmployeeAsync(ListingRequest request);
    }
}