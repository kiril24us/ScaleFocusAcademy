using System.Collections.Generic;
using ToDoApplication.Data.Entities;

namespace ToDoApplication.Data.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetUsernameByUsernameAndPassword(string username, string password);

        bool CheckIfUserExistById(int userId);

        User GetUserById(int userId);

        bool CheckIfUserIsCreatorOfToDoLists(int userId);
    }
}
