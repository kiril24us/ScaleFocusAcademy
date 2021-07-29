using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoApplication.Common;
using ToDoApplication.Data;
using ToDoApplication.Entities;

namespace ToDoApplication.Services
{

    public class UserService : IUserService
    {
        private const string StoreFileName = "Users.json";
        private const string StoreFileNameForCounterUsers = "CounterUsers.json";
        private int counterForCreateUsers;
        private readonly FileDatabase _storage;

        /// <summary>
        /// List of all application users
        /// </summary>
        public static List<User> _applicationUsers = new List<User>();

        /// <summary>
        /// Currently logged in user
        /// </summary>
        public User CurrentUser { get; private set; }

        public UserService()
        {
            _storage = new FileDatabase();
            List<User> usersFromFile = _storage.Read<List<User>>(StoreFileName);
            int counterUsersFromFile = _storage.ReadNumber(StoreFileNameForCounterUsers);

            if (usersFromFile == null)
            {
                CreateUser("admin", "adminpassword", null, null, 1);
            }
            else
            {
                _applicationUsers = usersFromFile;
            }

            counterForCreateUsers = counterUsersFromFile;
        }

        /// <summary>
        /// Creates new user
        /// </summary>
        /// <param name="name">The name of the new user</param>
        /// <returns>True if user created otherwise false</returns>
        public bool CreateUser(string name, string password, string firstname, string lastName, int role)
        {
            if (_applicationUsers.Any(x => x.Username == name))
            {
                return false;
            }

            var roleAsEnum = Enum.Parse<Role>(role.ToString());
            DateTime now = DateTime.Now;
            if (CurrentUser == null)
            {
                counterForCreateUsers++;
                _applicationUsers.Add(new User
                {
                    
                    Username = name,
                    Id = counterForCreateUsers,
                    DateOfCreation = now,
                    FirstName = firstname,
                    LastName = lastName,
                    Password = password,
                    Role = roleAsEnum,
                }) ;
                _storage.Write(StoreFileName, _applicationUsers);
                _storage.Write(StoreFileNameForCounterUsers, counterForCreateUsers);
                return true;
            }
            counterForCreateUsers++;
            _applicationUsers.Add(new User
            {
                Username = name,
                Id = counterForCreateUsers,
                DateOfCreation = now,
                FirstName = firstname,
                LastName = lastName,
                Password = password,
                Role = roleAsEnum,
                CreatorId = CurrentUser.Id,
            });

            _storage.Write(StoreFileName, _applicationUsers);
            _storage.Write(StoreFileNameForCounterUsers, counterForCreateUsers);
            return true;
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns>True if user is deleted otherwise false</returns>
        public bool DeleteUser(int userId)
        {
            bool isExist = CheckIfUserExistInTheStorage(userId);
            if (!isExist)
            {
                return false;
            }

            User user = FindUserById(userId);

            _applicationUsers.Remove(user);
            _storage.Write(StoreFileName, _applicationUsers);
            return true;
        }

        public User FindUserById(int userId)
        {
            return _applicationUsers.FirstOrDefault(x => x.Id == userId);
        }

        public List<User> GetAllUsers()
        {
            return _applicationUsers;
        }

        public bool EditUser(int userId, string username, string password, string firstname, string lastname)
        {
            bool isExist = CheckIfUserExistInTheStorage(userId);
            if (!isExist)
            {
                return false;
            }
            User user = FindUserById(userId);
            DateTime dateOfChange = DateTime.Now;

            user.Username = username;
            user.Password = password;
            user.FirstName = firstname;
            user.LastName = lastname;
            user.DateOfLastChange = dateOfChange;
            user.IdOfUserLastChange = CurrentUser.Id;

            _storage.Write(StoreFileName, _applicationUsers);
            return true;
        }

        public bool CheckIfUserExistInTheStorage(int userId)
        {
            if (!_applicationUsers.Any(x => x.Id == userId))
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
            CurrentUser = _applicationUsers.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public void Logout()
        {
            CurrentUser = null;
        }
    }
}
