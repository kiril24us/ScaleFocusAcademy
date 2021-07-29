using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Task = System.Threading.Tasks.Task;

namespace ProjectManagementApplication.Services.Services
{
    public class IdentityServerUserManager : UserManager<User>, IUserManager
    {
        public IdentityServerUserManager(IUserStore<User> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<User> passwordHasher,
            IEnumerable<IUserValidator<User>> userValidators,
            IEnumerable<IPasswordValidator<User>> passwordValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            IServiceProvider services,
            ILogger<UserManager<User>> logger) :
            base(store,
            optionsAccessor,
            passwordHasher,
            userValidators,
            passwordValidators,
            keyNormalizer,
            errors,
            services,
            logger)
        {

        }

        public async Task<User> GetTheUserAsync(ClaimsPrincipal claimsPrincipal)
        {
            return await GetUserAsync(claimsPrincipal);           
        }

        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return (await GetRolesAsync(user)).ToList();
        }

        public async Task CreateUserAsync(User user)
        {
            await CreateAsync(user);
        }

        public async Task AddRoleAsync(User user, string role)
        {
            await AddToRoleAsync(user, role);
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            User user = await FindByIdAsync(userId);
            return await IsInRoleAsync(user, roleName);
        }

        public async Task<bool> ValidateUserCredentials(string userName, string password)
        {
            User user = await FindByNameAsync(userName);
            if (user != null)
            {
                bool result = await CheckPasswordAsync(user, password);
                return result;
            }
            return false;
        }
    }
}
