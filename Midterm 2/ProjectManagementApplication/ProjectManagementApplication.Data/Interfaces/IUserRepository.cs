using ProjectManagementApplication.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface IUserRepository : IRepository<User> 
    {
        Task<User> GetUserByUsername(string username);

        Task<User> GetUserById(string userId);

        Task<List<User>> GetAll();
    }
}
