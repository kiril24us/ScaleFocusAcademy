using ProjectManagementApplication.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface IUserRepository : IRepository<User> 
    {
        Task<User> GetUserByUsernameAndPassword(string username, string password);

        Task<User> GetUserById(int userId);

        Task<List<User>> GetAll();
    }
}
