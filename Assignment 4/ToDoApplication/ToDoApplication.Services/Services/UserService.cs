using System;
using System.Collections.Generic;
using ToDoApplication.Data.Entities;
using ToDoApplication.Data.Enum;
using ToDoApplication.Data.Interfaces;
using ToDoApplication.Services.Interfaces;
using ToDoApplication.Services.Auth;

namespace ToDoApplication.Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name = "name" > The name of the new user</param>
        /// <returns>True if user created otherwise false</returns>
        public bool CreateUser(string username, string password, string firstname, string lastName, int role)
        {
            if (_userRepository.GetUsernameByUsernameAndPassword(username, password) != null)
            {
                return false;
            }

            var roleAsEnum = Enum.Parse<Role>(role.ToString());
            DateTime dateOfCreation = DateTime.Now;            

            return _userRepository.Create(new User
            {
                CreatedById = Authentication.CurrentUser.UserId,
                Username = username,
                Password = password,
                CreatedOn = dateOfCreation,
                LastModifiedOn = dateOfCreation,
                LastModifiedById = Authentication.CurrentUser.UserId,
                FirstName = firstname,
                LastName = lastName,
                Role = roleAsEnum,
            });
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if user is deleted otherwise false</returns>
        public bool DeleteUser(int userId)
        {
            bool isExist = _userRepository.CheckIfUserExistById(userId);
            if (!isExist)
            {
                return false;
            }

            User user = _userRepository.GetUserById(userId);

            return _userRepository.Delete(user);
        }       

        /// <summary>
        /// Get All Users in Database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers(int userId)
        {
            return _userRepository.GetAll(userId);
        }

        /// <summary>
        /// Edit User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="firstname"></param>
        /// <param name="lastname"></param>
        /// <returns>True if user is edited otherwise false</returns>
        public bool EditUser(int userId, string username, string password, string firstname, string lastname)
        {
            User userToEdit = _userRepository.GetUserById(userId);
            if(userToEdit == null)
            {
                return false;
            }
            userToEdit.Username = username;
            userToEdit.Password = password;
            userToEdit.FirstName = firstname;
            userToEdit.LastName = lastname;
            userToEdit.LastModifiedById = Authentication.CurrentUser.UserId;
            userToEdit.LastModifiedOn = DateTime.Now;

            return _userRepository.Edit();           
        }

        public bool CheckIfUserExistById(int userId)
        {
            if (!_userRepository.CheckIfUserExistById(userId))
            {
                return false;
            }

            return true;
        }

        public bool CheckIfUserIsCreatorOfToDoLists(int userId)
        {
            return _userRepository.CheckIfUserIsCreatorOfToDoLists(userId);
        }
      
        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }

        public User GetUsernameByUsernameAndPassword(string username, string password)
        {
            return _userRepository.GetUsernameByUsernameAndPassword(username, password);
        }
    }
}
