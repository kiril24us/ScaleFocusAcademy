using System.Collections.Generic;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.BLL.Services
{
    public interface IUserService
    {
        public bool CreateUser(string username, string password, string firstname, string lastName, int role);

        public bool DeleteUser(int userId);

        public List<User> GetAllUsers();

        public bool EditUser(int userId, string username, string password, string firstname, string lastname);

        public bool CheckIfUserExistById(int userId);

        public void Login(string username, string password);

        public void Logout();

        public User CurrentUser { get; }
    }
}
