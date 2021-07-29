using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.BLL.Services
{

    public class UserService : IUserService
    {
        private readonly UserDatabase _userDatabase;
      
        /// <summary>
        /// Currently logged in user
        /// </summary>
        public User CurrentUser { get; private set; }

        public UserService(UserDatabase userDatabase)
        {
            _userDatabase = userDatabase;         
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name = "name" > The name of the new user</param>
        /// <returns>True if user created otherwise false</returns>
        public bool CreateUser(string username, string password, string firstname, string lastName, int role)
        {
            if (_userDatabase.GetUserByUsername(username, password) != null)
            {
                return false;
            }

            var roleAsEnum = Enum.Parse<Role>(role.ToString());
            DateTime dateOfCreation = DateTime.Now;
            if (CurrentUser == null)
            {               
                _userDatabase.CreateUser(new User
                {
                    CreatorId = 0,
                    Username = username,
                    Password = password,
                    DateOfCreation = dateOfCreation,
                    FirstName = firstname,
                    LastName = lastName,
                    Role = roleAsEnum,
                    IdOfUserLastChange = 0,
                    DateOfLastChange = dateOfCreation,
                });
                Login(username, password);
                return true;
            }           

            return _userDatabase.CreateUser(new User
            {
                CreatorId = CurrentUser.Id,
                Username = username,
                Password = password,
                DateOfCreation = dateOfCreation,
                DateOfLastChange = dateOfCreation,
                IdOfUserLastChange = CurrentUser.Id,
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
            bool isExist = _userDatabase.CheckIfUserExistById(userId);
            if (!isExist)
            {
                return false;
            }

            _userDatabase.DeleteUser(userId);
            return true;
        }       

        /// <summary>
        /// Get All Users in Database
        /// </summary>
        /// <returns></returns>
        public List<User> GetAllUsers()
        {
            return _userDatabase.GetAllUsers();
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
            DateTime dateOfChange = DateTime.Now;
            int IdOfUserLastChange = CurrentUser.Id;
            return _userDatabase.EditUser(userId, username, password, firstname, lastname, dateOfChange, IdOfUserLastChange);           
        }

        public bool CheckIfUserExistById(int userId)
        {
            if (!_userDatabase.CheckIfUserExistById(userId))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Login the user in the system and keep the data in the CurrentUser variable
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(string username, string password)
        {
            CurrentUser = _userDatabase.GetUserByUsername(username, password);
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
