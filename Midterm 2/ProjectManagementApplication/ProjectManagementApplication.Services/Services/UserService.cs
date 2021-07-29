using Microsoft.AspNetCore.Identity;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;
        private readonly IUserManager _userManager;

        public UserService(IUserRepository userRepository, ITeamRepository teamRepository, IUserManager userManager)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
            _userManager = userManager;
        }


        public async Task<List<string>> GetUserRolesAsync(User user)
        {
            return await _userManager.GetUserRolesAsync(user);
        }

        public async Task<User> GetCurrentUser(ClaimsPrincipal principal)
        {
            return await _userManager.GetTheUserAsync(principal);
        }

        public async Task<User> GetUserByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<User> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<bool> IsUserInRole(string userId, string roleName)
        {
            return await _userManager.IsUserInRole(userId, roleName);
        }

        /// <summary>
        /// Create User
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <param name="role"></param>
        /// <returns>Returns Success if User is created or a Message Error</returns>
        public async Task<Messages> CreateUser(string username, string password, string firstName, string lastName, int role, int teamId)
        {
            if (await _userManager.FindByNameAsync(username) != null)
            {
                return Messages.ChangeUsernameOrPassword;
            }

            string userRole;

            if (role == 1)
            {
                userRole = "Admin";
            }
            else
            {
                userRole = "Manager";
            }

            Team team = await _teamRepository.GetTeamById(teamId);           

            User newUser = new User
            {
                Email = "test@test.test",
                NormalizedEmail = "test@test.test".ToUpper(),
                EmailConfirmed = true,
                UserName = username,                
                FirstName = firstName,
                LastName = lastName,
                IsActive = true,
                Teams = { team },
                NormalizedUserName = username.ToUpper(),
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            newUser.PasswordHash = hasher.HashPassword(newUser, password);

            try
            {
                await _userManager.CreateUserAsync(newUser);
            }
            catch (Exception)
            {
                return Messages.TeamNotFound;
            }

            try
            {
                await _userManager.AddRoleAsync(newUser, userRole);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            };
            return Messages.Success;
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if User is deleted otherwise false</returns>
        public async Task<Messages> DeleteUser(string userId, string currentUserId)
        {
            if (userId == currentUserId)
            {
                return Messages.YouCannotDeleteSameUserYouAreLoggedIn;
            }

            User userToDelete = await _userRepository.GetUserById(userId);

            if (userToDelete == null)
            {
                return Messages.UserNotFound;
            }

            userToDelete.IsActive = false;

            if (await _userRepository.Edit())
            {
                return Messages.Success;
            }

            return Messages.OperationWasNotSuccessful;
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns>True if User is edited otherwise false</returns>
        public async Task<bool> EditUser(string userId, string username, string password, string firstName, string lastName)
        {
            User userToEdit = await _userRepository.GetUserById(userId);

            if (userToEdit == null)
            {
                return false;
            }

            PasswordHasher<User> hasher = new PasswordHasher<User>();

            userToEdit.UserName = username;
            userToEdit.PasswordHash = hasher.HashPassword(userToEdit, password);
            userToEdit.FirstName = firstName;
            userToEdit.LastName = lastName;

            return await _userRepository.Edit();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }
    }
}
