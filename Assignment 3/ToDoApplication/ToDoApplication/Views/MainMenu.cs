using System;
using ToDoApplication.ApplicationMethods;
using ToDoApplication.BLL.Services;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Repositories;

namespace ToDoApplication.Views
{
    public class MainMenu
    {
        private static IUserService _userService;
        private static IToDoListService _toDoListService;
        private static ITaskService _taskService;
        private static IUserRepository _userRepository;
        private static IToDoListRepository _toDoListRepository;
        private static ITaskRepository _taskRepository;
        private static AppDbContext _applicationContext = new AppDbContext();
        private static UserMethods _userMethods;
        private static ToDoListMethods _toDoListMethods;
        private static TaskMethods _taskMethods;

        public MainMenu()
        {
            _userRepository = new UserRepository(_applicationContext);
            _toDoListRepository = new ToDoListRepository(_applicationContext);
            _taskRepository = new TaskRepository(_applicationContext);

            _userService = new UserService(_userRepository);
            _userService.Login("admin", "adminpassword");
            _toDoListService = new ToDoListService(_toDoListRepository, _userRepository, _taskRepository);
            _taskService = new TaskService(_taskRepository);

            _userMethods = new UserMethods(_userService, _toDoListService);
            _toDoListMethods = new ToDoListMethods(_toDoListService, _userService);
            _taskMethods = new TaskMethods(_taskService, _toDoListService, _userService);
        }

        public bool MainMenuView()
        {            
            RenderMenu();
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    if (_userService.CurrentUser == null)
                    {
                        _userMethods.Login();
                    }
                    else
                    {
                        while (!UserManagement.UserManagementMenu(_userService.CurrentUser.Role.ToString(), _userMethods));
                    }
                    return false;
                case "2":
                    while (!ToDoListManagement.ToDoListManagementMenu(_toDoListMethods)) ;
                    return false;
                case "3":
                    while (!TaskManagement.TaskManagementMenu(_taskMethods)) ;
                    return false;
                case "exit":
                    return true;
                default:
                    Console.WriteLine("Unknown commands!");
                    return false;
            }
        }

        private static void RenderMenu()
        {
            Console.WriteLine("--------Main Menu--------");
            if (_userService.CurrentUser == null)
            {
                Console.WriteLine("1. LogIn ");
                return;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"You are logged in: {_userService.CurrentUser.Username}");
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine("1. User Management ");
                Console.WriteLine("2. ToDo List Management ");
                Console.WriteLine("3. Task Management ");
            }
        }
    }
}
