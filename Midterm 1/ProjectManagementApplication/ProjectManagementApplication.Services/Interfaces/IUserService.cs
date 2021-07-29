using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Services.Enums;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface IUserService
    {
        Task<Messages> CreateUser(string username, string password, string firstName, string lastName, int role, int teamId);

        Task<bool> DeleteUser(int userId);

        Task<bool> EditUser(int userId, string username, string password, string firstName, string lastName);

        Task<User> GetUserByUsernameAndPassword(string username, string password);

        Task<List<User>> GetAllUsers();
    }
}
