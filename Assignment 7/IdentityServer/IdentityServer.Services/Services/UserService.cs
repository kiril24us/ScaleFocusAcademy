using IdentityServer.Data.Entities;
using IdentityServer.Services.Interfaces;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace IdentityServer.Services.Services
{
    /// <summary>
    /// Responsible for managing user related functionality and tracking currently logged in user
    /// </summary>
    public class UserService : IUserService
    {
        private readonly IUserManager _userManager;

        /// <summary>
        /// Initializes new instance of the UserService and creates a single default user 
        /// </summary>
        public UserService(IUserManager userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        /// <param name="name">The name of the new User</param>
        /// <returns>True if user created otherwise false </returns>
        public async Task<bool> CreateUser(string userName, string password, int role)
        {
            if (await _userManager.FindByNameAsync(userName) != null)
            {
                return false;
            }

            string userRole;

            if(role == 1)
            {
                userRole = "Admin";
            }
            else
            {
                userRole = "Regular";
            }

            User user = new User()
            {
                UserName = userName                
            };           

            await _userManager.CreateUserAsync(user, password);
            await _userManager.AddRoleAsync(user, userRole);

            return true;
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetUserAsync(principal);
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<List<User>> GetAll()
        {
            return await _userManager.GetAllAsync();
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            return await _userManager.IsUserInRole(userId, roleName);
        }
    }
}
