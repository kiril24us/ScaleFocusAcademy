using ProjectManagementApplication.Data.Entities;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface IAuthentication
    {
        static User CurrentUser { get; }

        Task<User> Login(string username, string password);
    }
}
