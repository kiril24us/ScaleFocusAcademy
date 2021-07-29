using System;
using ToDoApplication.ApplicationMethods;

namespace ToDoApplication.Views
{
    public class UserManagement
    {
        private static MainMenu _mainMenu = new MainMenu();

        public static bool UserManagementMenu(string role, UserMethods userMethods)
        {
            RenderUserMenu(role);

            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    userMethods.Logout();
                    return true;
                case "2":
                    if (role == "Admin")
                    {
                        userMethods.CreateUser();
                    }
                    else
                    {
                        userMethods.ListAllUsers();
                    }
                    return false;
                case "3":
                    if (role == "Admin")
                    {
                        userMethods.DeleteUser();
                        return false;
                    }
                    else
                    {
                        _mainMenu.MainMenuView();
                        return true;
                    }
                case "4":
                    if (role == "Admin")
                    {
                        userMethods.EditUser();
                    }
                    return false;
                case "5":
                    if (role == "Admin")
                    {
                        userMethods.ListAllUsers();
                    }
                    return false;
                case "6":
                    if (role == "Admin")
                    {
                        _mainMenu.MainMenuView();
                        return true;
                    }
                    return false;
                case "exit":
                    return true;
                default:
                    Console.WriteLine("Unknown commands!");
                    return false;
            }
        }

        private static void RenderUserMenu(string userRole)
        {
            Console.WriteLine("=======================================================");
            Console.WriteLine("1. Log out ");

            if (userRole == "Admin")
            {
                Console.WriteLine("2. Create User ");
                Console.WriteLine("3. Delete User ");
                Console.WriteLine("4. Edit User ");
                Console.WriteLine("5. List all Users ");
                Console.WriteLine("6. Back to Main Menu ");
            }
            else
            {
                Console.WriteLine("2. List all Users ");
                Console.WriteLine("3. Back to Main Menu ");
            }
        }
    }
}
