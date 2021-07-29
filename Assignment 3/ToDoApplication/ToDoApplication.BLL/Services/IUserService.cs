using System.Collections.Generic;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.BLL.Services
{
    public interface IUserService
    {
        bool CreateUser(string username, string password, string firstname, string lastName, int role);

        bool DeleteUser(int userId);

        List<User> GetAllUsers();

        bool EditUser(int userId, string username, string password, string firstname, string lastname);

        void Login(string username, string password);

        void Logout();

        bool CheckIfUserExistById(int userId);

        bool CheckIfUserIsCreatorOfToDoLists(int userId);

        User GetUserById(int userId);

        User CurrentUser { get; }
    }
}
