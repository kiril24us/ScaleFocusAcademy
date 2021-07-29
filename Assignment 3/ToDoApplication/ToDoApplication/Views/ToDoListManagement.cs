using System;
using ToDoApplication.ApplicationMethods;

namespace ToDoApplication.Views
{
    public class ToDoListManagement
    {
        private static MainMenu _mainMenu = new MainMenu();

        public static bool ToDoListManagementMenu(ToDoListMethods toDoListMethods)
        {
            RenderToDoListMenu();
            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    toDoListMethods.CreateToDoList();
                    return false;
                case "2":
                    toDoListMethods.DeleteToDoList();
                    return false;
                case "3":
                    toDoListMethods.EditToDoList();
                    return false;
                case "4":
                    toDoListMethods.ShareToDoList();
                    return false;
                case "5":
                    toDoListMethods.ListAllToDoListsOfUser();
                    return false;
                case "6":
                    _mainMenu.MainMenuView();
                    return true;
                case "exit":
                    return true;
                default:
                    Console.WriteLine("Unknown commands!");
                    return false;
            }
        }

        private static void RenderToDoListMenu()
        {
            Console.WriteLine("=======================================================");
            Console.WriteLine("1. Create ToDo list ");
            Console.WriteLine("2. Delete ToDo list ");
            Console.WriteLine("3. Edit ToDo list ");
            Console.WriteLine("4. Share ToDo list with other User ");
            Console.WriteLine("5. List all ToDo lists ");
            Console.WriteLine("6. Back to Main Menu ");
        }
    }
}
