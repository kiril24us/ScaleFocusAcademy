using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Interfaces;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Auth
{
    public class Authentication : IAuthentication
    {
        private static IUserRepository _userRepository;

        public Authentication(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Currently logged in User
        /// </summary>
        public static User CurrentUser { get; private set; }

        /// <summary>
        /// Login the user in the system and keep the data in the CurrentUser variable
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public async Task<User> Login(string username, string password)
        {
            return CurrentUser =  await _userRepository.GetUserByUsernameAndPassword(username, password);
        }
    }
}
