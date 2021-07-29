using System.Collections.Generic;
using ToDoApplication.Data.Entities;

namespace ToDoApplication.Services.Interfaces
{
    public interface IUserService
    {
        bool CreateUser(string username, string password, string firstname, string lastName, int role);

        bool DeleteUser(int userId);

        List<User> GetAllUsers(int userId);

        bool EditUser(int userId, string username, string password, string firstname, string lastname);

        bool CheckIfUserExistById(int userId);

        bool CheckIfUserIsCreatorOfToDoLists(int userId);

        User GetUserById(int userId);

        User GetUsernameByUsernameAndPassword(string username, string password);
    }
}
