using ProjectManagementApplication.Data.Entities;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface IUserManager
    {
        Task<User> GetTheUserAsync(ClaimsPrincipal claimsPrincipal);

        Task<bool> IsUserInRole(string userId, string roleId);

        Task<User> FindByNameAsync(string name);

        Task<User> FindByIdAsync(string id);

        Task AddRoleAsync(User user, string role);

        Task CreateUserAsync(User user);

        Task<List<string>> GetUserRolesAsync(User user);

        Task<bool> ValidateUserCredentials(string userName, string password);
    }
}
