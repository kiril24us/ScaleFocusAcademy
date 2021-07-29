using IdentityServer.Data.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services.Interfaces
{
    public interface IUserManager
    {
        Task<User> GetUserAsync(ClaimsPrincipal claimsPrincipal);

        Task<bool> IsUserInRole(string userId, string roleId);

        Task<User> FindByNameAsync(string name);

        Task<User> FindByIdAsync(string id);

        Task<List<User>> GetAllAsync();

        Task AddRoleAsync(User user, string role);

        Task CreateUserAsync(User user, string password);

        Task<List<string>> GetUserRolesAsync(User user);

        Task<bool> ValidateUserCredentials(string userName, string password);
    }
}
