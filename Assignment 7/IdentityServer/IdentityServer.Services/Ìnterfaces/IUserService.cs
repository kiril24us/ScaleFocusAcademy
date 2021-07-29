using IdentityServer.Data.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetCurrentUser(ClaimsPrincipal principal);

        Task<bool> IsUserInRole(string userId, string roleId);

        Task<bool> CreateUser(string userName, string password, int role);

        Task<List<User>> GetAll();

        Task<User> GetUserById(string id);

        Task<User> GetUserByName(string name);
    }
}