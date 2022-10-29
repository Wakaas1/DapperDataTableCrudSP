using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DapperStoredProc.Services
{
    public interface ICustomClaimsCookieSignInHelper<TIdentityUser> where TIdentityUser : IdentityUser
    {
        Task SignInUserAsync(TIdentityUser user, bool isPersistent, IEnumerable<Claim> customClaims);
    }
}