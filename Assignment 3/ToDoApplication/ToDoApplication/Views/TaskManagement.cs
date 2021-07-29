using System;
using ToDoApplication.ApplicationMethods;

namespace ToDoApplication.Views
{
    public class TaskManagement
    {
        private static MainMenu _mainMenu = new MainMenu();

        public static bool TaskManagementMenu(TaskMethods taskMethods)
        {
            RenderTaskMenu();
            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    taskMethods.CreateTask();
                    return false;
                case "2":
                    taskMethods.DeleteTask();
                    return false;
                case "3":
                    taskMethods.EditTask();
                    return false;
                case "4":
                    taskMethods.AssignTask();
                    return false;
                case "5":
                    taskMethods.CompleteTask();
                    return false;
                case "6":
                    taskMethods.ListAllTasks();
                    return false;
                case "7":
                    _mainMenu.MainMenuView();
                    return true;
                case "exit":
                    return true;
                default:
                    Console.WriteLine("Unknown commands!");
                    return false;
            }
        }

        private static void RenderTaskMenu()
        {
            Console.WriteLine("=======================================================");
            Console.WriteLine("1. Create a Task ");
            Console.WriteLine("2. Delete a Task ");
            Console.WriteLine("3. Edit a Task ");
            Console.WriteLine("4. Assign a Task ");
            Console.WriteLine("5. Complete a Task ");
            Console.WriteLine("6. List all Tasks in a single To Do List ");
            Console.WriteLine("7. Back to Main Menu ");
        }
    }
}
