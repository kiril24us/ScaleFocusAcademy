using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Services.Enums;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<string>> GetUserRolesAsync(User user);

        Task<User> GetCurrentUser(ClaimsPrincipal principal);

        Task<bool> IsUserInRole(string userId, string roleId);

        Task<User> GetUserById(string id);

        Task<User> GetUserByName(string name);

        Task<Messages> CreateUser(string username, string password, string firstName, string lastName, int role, int teamId);

        Task<Messages> DeleteUser(string userId, string currentUserId);

        Task<bool> EditUser(string userId, string username, string password, string firstName, string lastName);

        Task<List<User>> GetAllUsers();
    }
}
