using Microsoft.Extensions.Primitives;
using ToDoApplication.Data.Entities;
using ToDoApplication.Data.Interfaces;
using ToDoApplication.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace ToDoApplication.Services.Auth
{
    public static class Authentication
    {
        /// <summary>
        /// Currently logged in User
        /// </summary>
        public static User CurrentUser { get; private set; }

        public static User GetCurrentUser(this IUserService userService, HttpRequest httpRequest)
        {
            StringValues authHeaders;

            httpRequest.Headers.TryGetValue("AuthenticationUsernameId", out authHeaders);

            if (authHeaders.Count != 0)
            {
                int usernameId = int.Parse(authHeaders.First());

                return CurrentUser = userService.GetUserById(usernameId);
            }

            return null;
        }

        /// <summary>
        /// Login the user in the system and keep the data in the CurrentUser variable
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public static void Login(string username, string password, IUserRepository userRepository)
        {
            CurrentUser = userRepository.GetUsernameByUsernameAndPassword(username, password);
        }

        public static void Logout()
        {
            CurrentUser = null;
        }
    }
}
