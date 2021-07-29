using System.Collections.Generic;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUsernameByUsernameAndPassword(string username, string password);

        bool CheckIfUserExistById(int userId);

        User GetUserById(int userId);

        List<User> GetAllUsers();

        bool CheckIfUserIsCreatorOfToDoLists(int userId);
    }
}
