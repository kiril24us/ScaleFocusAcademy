using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using ToDoApplication.BLL.Services;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.ApplicationMethods
{
    public class UserMethods
    {
        private static IUserService _userService;
        private readonly IToDoListService _toDoListService;

        public UserMethods(IUserService userService, IToDoListService toDoListService)
        {
            _userService = userService;
            _toDoListService = toDoListService;
        }

        public void Login()
        {
            Console.WriteLine("Write down your username please!");
            string username = Console.ReadLine();
            Console.WriteLine("Write down your password please!");
            string password = Console.ReadLine();
            _userService.Login(username, password);
            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("Login failed!");
            }
            else
            {
                Console.WriteLine("Login successful!");
            }
        }

        public void Logout()
        {
            _userService.Logout();
        }

        public void CreateUser()
        {
            string username, password, firstname, lastname;
            AskForInformationAboutTheUser(out username, out password, out firstname, out lastname);
            Console.WriteLine("Choose role for the user, write 1 for administrative privileges OR 2 for without administrative privileges");
            int role = CommonMethods.ReadAnIntFromTheConsole();
            role = CheckIfParameterIs1Or2(role);
            bool isSuccess = _userService.CreateUser(username, password, firstname, lastname, role);

            if (isSuccess)
            {
                Console.WriteLine($"User with username {username} was successfully created!");
            }
            else
            {
                Console.WriteLine($"Already exists user with name {username}!");
            }
        }

        public void EditUser()
        {
            Console.WriteLine("Write Id of the user who want to be edited!");
            int userId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _userService.CheckIfUserExistById(userId);
            if (isExist)
            {
                string newUsername, newPassword, newFirstName, newLastName;
                AskForInformationAboutTheUser(out newUsername, out newPassword, out newFirstName, out newLastName);
                _userService.EditUser(userId, newUsername, newPassword, newFirstName, newLastName);
                Console.WriteLine($"You successfully edited the user with id {userId}");
            }
            else
            {
                Console.WriteLine($"User with id {userId} was not found!");
            }
        }

        public void DeleteUser()
        {
            Console.WriteLine("Write Id of the user who want to be deleted!");
            int userId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _userService.CheckIfUserIsCreatorOfToDoLists(userId);

            if (isExist)
            {
                User user = _userService.GetUserById(userId);
                int countOfToDoListsOfUser = user.ToDoLists.Count;
                for (int i = 0; i < countOfToDoListsOfUser; i++)
                {
                    int toDoListIdToDelete = user.ToDoLists.Select(x => x.ToDoListId).FirstOrDefault();
                    _toDoListService.DeleteToDolist(toDoListIdToDelete, _userService.CurrentUser.UserId);
                }
            }
            bool isSuccess = _userService.DeleteUser(userId);
            if (isSuccess)
            {
                Console.WriteLine($"User with id {userId} was successfully deleted!");
            }
            else
            {
                Console.WriteLine($"User with id {userId} was not found!");
            }
        }

        public void ListAllUsers()
        {
            List<User> allUsers = _userService.GetAllUsers();
            if (allUsers.Any())
            {
                foreach (User user in allUsers)
                {
                    Console.WriteLine($"========================={user.UserId}===========================");
                    Console.WriteLine($"Username: {user.Username}");
                    Console.WriteLine($"Firstname: {user.FirstName}");
                    Console.WriteLine($"Lastname: {user.LastName}");
                    Console.WriteLine($"Role of the User: {user.Role}");
                    Console.WriteLine($"User with id {user.CreatedById} created the user");
                    Console.WriteLine($"The user was created on {user.CreatedOn.ToString("D", CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"The last change of the user was on {user.LastModifiedOn.ToString("D", CultureInfo.InvariantCulture)} and was done by user with id {user.LastModifiedById}");
                    Console.WriteLine("=======================================================");
                }
            }
            else
            {
                Console.WriteLine("=======================================================");
                Console.WriteLine("No users was found");
                Console.WriteLine("=======================================================");

            }
        }

        private static void AskForInformationAboutTheUser(out string newUsername, out string newPassword, out string newFirstName, out string newLastName)
        {
            Console.WriteLine("Write the new username of the user");
            newUsername = Console.ReadLine();
            Console.WriteLine("Write the new password of the user");
            newPassword = Console.ReadLine();
            Console.WriteLine("Write the new firstname of the user");
            newFirstName = Console.ReadLine();
            Console.WriteLine("Write the new lastname of the user");
            newLastName = Console.ReadLine();
        }

        private static int CheckIfParameterIs1Or2(int parameter)
        {
            while (parameter != 1 && parameter != 2)
            {
                Console.WriteLine("Write 1 or 2, please!");
                parameter = CommonMethods.ReadAnIntFromTheConsole();
            }

            return parameter;
        }
    }
}
