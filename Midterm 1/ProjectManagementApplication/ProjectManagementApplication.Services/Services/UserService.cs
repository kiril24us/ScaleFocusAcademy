using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Enums;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITeamRepository _teamRepository;

        public UserService(IUserRepository userRepository, ITeamRepository teamRepository)
        {
            _userRepository = userRepository;
            _teamRepository = teamRepository;
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
            if (await _userRepository.GetUserByUsernameAndPassword(username, password) != null)
            {
                return Messages.ChangeUsernameOrPassword;
            }

            Role roleAsEnum = Enum.Parse<Role>(role.ToString());
            Team team = await _teamRepository.GetTeamById(teamId);

            try
            {
                await _userRepository.Create(new User
                {
                    Username = username,
                    Password = password,
                    FirstName = firstName,
                    LastName = lastName,
                    Role = roleAsEnum,
                    IsActive = true,
                    Teams = { team }
                });
            }
            catch (Exception)
            {
                return Messages.TeamNotFound;
            }

            return Messages.Success;
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if User is deleted otherwise false</returns>
        public async Task<bool> DeleteUser(int userId)
        {
            User userToDelete = await _userRepository.GetUserById(userId);

            if (userToDelete == null)
            {
                return false;
            }

            userToDelete.IsActive = false;

            return await _userRepository.Edit();
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
        public async Task<bool> EditUser(int userId, string username, string password, string firstName, string lastName)
        {
            User userToEdit = await _userRepository.GetUserById(userId);

            if (userToEdit == null)
            {
                return false;
            }

            userToEdit.Username = username;
            userToEdit.Password = password;
            userToEdit.FirstName = firstName;
            userToEdit.LastName = lastName;

            return await _userRepository.Edit();
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await _userRepository.GetAll();
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _userRepository.GetUserByUsernameAndPassword(username, password);
        }
    }
}
