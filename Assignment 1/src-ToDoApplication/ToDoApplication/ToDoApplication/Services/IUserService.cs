using System;
using System.Collections.Generic;
using System.Text;
using ToDoApplication.Entities;

namespace ToDoApplication.Services
{
    public interface IUserService
    {
        public bool CreateUser(string name, string password, string firstname, string lastName, int role);

        public bool DeleteUser(int userId);

        public User FindUserById(int userId);

        public List<User> GetAllUsers();

        public bool EditUser(int userId, string username, string password, string firstname, string lastname);

        public bool CheckIfUserExistInTheStorage(int userId);

        public void Login(string username, string password);

        public void Logout();

        public User CurrentUser { get; }
    }
}
