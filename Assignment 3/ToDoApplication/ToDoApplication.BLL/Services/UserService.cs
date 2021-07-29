using System;
using System.Collections.Generic;
using ToDoApplication.DAL.Entities;
using ToDoApplication.DAL.Enum;
using ToDoApplication.DAL.Repositories;

namespace ToDoApplication.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        /// <summary>
        /// Currently logged in user
        /// </summary>
        public User CurrentUser { get; private set; }

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
                CreatedById = CurrentUser.UserId,
                Username = username,
                Password = password,
                CreatedOn = dateOfCreation,
                LastModifiedOn = dateOfCreation,
                LastModifiedById = CurrentUser.UserId,
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
            object result = _userRepository.Delete(user);
            return result != null;
        }       

        /// <summary>
        /// Get All Users in Database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
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
            userToEdit.LastModifiedById = CurrentUser.UserId;
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

        /// <summary>
        /// Login the user in the system and keep the data in the CurrentUser variable
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(string username, string password)
        {
            CurrentUser = _userRepository.GetUsernameByUsernameAndPassword(username, password);
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }
    }
}
