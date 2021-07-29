using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using ToDoApplication.BLL.Services;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication
{
    class Program
    {
        private static IUserService _userService;
        private static IToDoListService _toDoListService;
        private static ITaskService _taskService;
        private static IConfigurationRoot _configuration;

        static void Main(string[] args)
        {
            InitializeApplication();

            if (args.Length > 0)
            {
                _userService.Login(args[0], args[1]);
            }

            if (_userService.CurrentUser == null)
            {
                _userService.CreateUser("admin", "adminpassword", "", "", 1);
            }

            bool shouldExit = false;
            
            while (!shouldExit)
            {
                shouldExit = MainMenu();
            }
        }

        static void InitializeApplication()
        {
            // Read config file
            _configuration = ConfigInitializer.InitConfig();

            string connectionString = _configuration.GetConnectionString("Default");
            bool isExist = DatabaseExistance.CheckIfDatabaseExists(connectionString);
            //Create new database and tables 
            if (!isExist)
            {
                CreateDatabase.Create(connectionString);
            }

            _configuration = ConfigInitializer.AfterInitConfig();
            connectionString = _configuration.GetConnectionString("After_Initializing");
            UserDatabase userDatabase = new UserDatabase(connectionString);
            ToDoListDatabase toDoListDatabase = new ToDoListDatabase(connectionString);
            TaskDatabase taskDatabase = new TaskDatabase(connectionString);
            _userService = new UserService(userDatabase);
            _toDoListService = new ToDoListService(toDoListDatabase);
            _taskService = new TaskService(taskDatabase);
        }

        private static bool MainMenu()
        {
            RenderMenu();
            string userChoice = Console.ReadLine();

            switch (userChoice)
            {
                case "1":
                    if (_userService.CurrentUser == null)
                    {
                        Login();
                    }
                    else
                    {
                        while(!UserManagementMenu());
                    }
                    return false;
                case "2":
                    while(!ToDoListManagementMenu());
                    return false;
                case "3":
                    while(!TaskManagementMenu());
                    return false;
                case "exit":
                    return true;
                default:
                    Console.WriteLine("Unknown commands!");
                    return false;
            }
        }

        private static bool TaskManagementMenu()
        {
            RenderTaskMenu();
            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    CreateTask();
                    return false;
                case "2":
                    DeleteTask();
                    return false;
                case "3":
                    EditTask();
                    return false;
                case "4":
                    AssignTask();
                    return false;
                case "5":
                    CompleteTask();
                    return false;
                case "6":
                    ListAllTasks();
                    return false;
                case "7":
                    MainMenu();
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

        private static bool ToDoListManagementMenu()
        {
            RenderToDoListMenu();
            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    CreateToDoList();
                    return false;
                case "2":
                    DeleteToDoList();
                    return false;
                case "3":
                    EditToDoList();
                    return false;
                case "4":
                    ShareToDoList();
                    return false;
                case "5":
                    ListAllToDoLists();
                    return false;
                case "6":
                    MainMenu();
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

        private static bool UserManagementMenu()
        {
            string role = _userService.CurrentUser.Role.ToString();
            RenderUserMenu(role);
            
            string userChoice = Console.ReadLine();
            switch (userChoice)
            {
                case "1":
                    Logout();
                    return true;
                case "2":
                    if (role == "Admin")
                    {
                        CreateUser();
                    }
                    else
                    {
                        ListAllUsers();
                    }
                    return false;
                case "3":
                    if (role == "Admin")
                    {
                        DeleteUser();
                        return false;
                    }
                    else
                    {
                        MainMenu();
                        return true;
                    }
                    
                case "4":
                    if (role == "Admin")
                    {
                        EditUser();
                    }
                    return false;
                case "5":
                    if (role == "Admin")
                    {
                        ListAllUsers();
                    }
                    return false;
                case "6":
                    if (role == "Admin")
                    {
                        MainMenu();
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

        private static void CompleteTask()
        {
            Console.WriteLine("Write the Task Id which you want to complete");
            int taskId = ReadAnIntFromTheConsole();

            bool isExist = _taskService.CheckIfTaskExistInDatabaseById(taskId);
            if (isExist)
            {
                int toDoListId = _toDoListService.GetToDoListIdWhichContainsTask(taskId);
                if (toDoListId > 0)
                {
                    bool isExistToDoListInUser = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, _userService.CurrentUser.Id);
                    bool isExistSharedToDoListInUser = _toDoListService.CheckIfToDoListExistInSharedListsOfUser(toDoListId, _userService.CurrentUser.Id);
                    if (isExistToDoListInUser || isExistSharedToDoListInUser)
                    {
                        bool isSuccess = _taskService.CompleteTask(taskId);
                        if (isSuccess)
                        {
                            Console.WriteLine($"You successfully completed task with id {taskId} ");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"No Tasks with id {taskId} was found");
            }
        }

        private static void AssignTask()
        {
            Console.WriteLine("Write Id of the Task which you want to be assigned");
            int taskId = ReadAnIntFromTheConsole();
            bool isExist = _taskService.CheckIfTaskExistInDatabaseById(taskId);

            if (isExist)
            {
                Console.WriteLine($"Write id of the user to assign task with id {taskId}");
                int userId = ReadAnIntFromTheConsole();
                int toDoListId = _toDoListService.GetToDoListIdWhichContainsTask(taskId);
                if (toDoListId > 0)
                {
                    bool isExistToDoListInUser = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, userId);
                    bool isExistSharedToDoListInUser = _toDoListService.CheckIfToDoListExistInSharedListsOfUser(toDoListId, userId);
                    if (isExistToDoListInUser || isExistSharedToDoListInUser)
                    {
                        bool isSuccess = _taskService.AssignTask(userId, taskId);
                        if (isSuccess)
                        {
                            ConsoleWriteSuccessfullyAssignTask(taskId, userId);
                        }
                        else
                        {
                            Console.WriteLine($"You already assign a Task with Id {taskId} to User with id {userId} ");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"User with id {userId} doesn't have ToDo List with id {toDoListId} ");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Task with id {taskId} was not found ");
            }
        }

        private static void ConsoleWriteSuccessfullyAssignTask(int taskId, int userId)
        {
            Console.WriteLine($"You successfully assign task with id {taskId} to user with id {userId}");
        }

        private static void ToDoListNotExist(int toDoListId)
        {
            Console.WriteLine($"To Do list with id {toDoListId} was not found!");
        }

        private static void ListAllTasks()
        {
            Console.WriteLine("Write Id of the ToDo List from which you want to list all Tasks!");
            int toDoListId = ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, _userService.CurrentUser.Id);
            bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedListsOfUser(toDoListId, _userService.CurrentUser.Id);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                List<Task> tasks = _taskService.GetAllTasks(toDoListId);
                PrintInfoForTasks(toDoListId, tasks);
            }
            else
            {
                ToDoListNotExist(toDoListId);
            }
        }

        private static void PrintInfoForTasks(int toDoListId, List<Task> tasks)
        {
            if (tasks.Any())
            {
                Console.WriteLine($"Next lines are all Tasks in To Do List with id {toDoListId}");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"========================={task.Id}===========================");
                    Console.WriteLine($"Task title is {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Task is complete - {task.IsComplete}");
                    Console.WriteLine($"Task was created on {task.DateOfCreation.ToString("D", CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"The last change of the Task was on {task.DateOfLastChange.ToString("D", CultureInfo.InvariantCulture)} and was done by user with id {task.IdOfUserLastChange}");
                    Console.WriteLine($"Creator of the Task is user with id - {task.CreatorId}");
                    Console.WriteLine("=======================================================");
                }
            }
            else
            {
                Console.WriteLine("=======================================================");
                Console.WriteLine($"No tasks in To Do list with id {toDoListId} was found!");
                Console.WriteLine("=======================================================");

            }
        }

        private static void EditTask()
        {
            Console.WriteLine("Write Id of the ToDo List from which you want to edit a Task!");
            int toDoListId = ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, _userService.CurrentUser.Id);
            bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedListsOfUser(toDoListId, _userService.CurrentUser.Id);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                Console.WriteLine("Write Id of the Task which you want to be edited!");
                int taskId = ReadAnIntFromTheConsole();
                if (_taskService.CheckIfTaskExistInDatabaseById(taskId))
                {
                    Console.WriteLine("Write new title for the Task");
                    string title = Console.ReadLine();
                    title = CheckIfTitleIsCorrect(title);
                    Console.WriteLine("Write new description for the Task");
                    string description = Console.ReadLine();
                    description = CheckIfTitleIsCorrect(description);
                    Console.WriteLine("Is your Task complete or not, write Y or N");
                    bool isComplete = ReadingBooleanFromConsole();
                    bool isSuccess = _taskService.EditTask(taskId, title, description, isComplete, _userService.CurrentUser.Id);
                    if (isSuccess)
                    {
                        Console.WriteLine($"Task with id {taskId} was successfully edited!");
                    }

                }
                else
                {
                    Console.WriteLine($"Task with id {taskId} was not found!");
                }
            }
            else
            {
                ToDoListNotExist(toDoListId);
            }
        }

        private static void DeleteTask()
        {
            Console.WriteLine("Write Id of the ToDo List from which you want to delete a Task!");
            int toDoListId = ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, _userService.CurrentUser.Id);
            bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedListsOfUser(toDoListId, _userService.CurrentUser.Id);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                Console.WriteLine("Write Id of the Task which you want to be deleted!");
                int taskId = ReadAnIntFromTheConsole();
                bool isExistTask = _taskService.CheckIfTaskExistInDatabaseById(taskId);
                if (isExistTask)
                {
                    bool isCreator = _taskService.CheckIfUserIsCreatorOfTheTask(taskId, _userService.CurrentUser.Id);
                    if (isCreator)
                    {
                        bool isSuccess = _taskService.DeleteTask(toDoListId, taskId);
                        if (isSuccess)
                        {
                            ConsoleWriteSuccessfullyDeletedTask(taskId);
                        }
                        else
                        {
                            ConsoleWriteWasntSuccessfullyDeletedTask(taskId);
                        }
                    }
                    else
                    {
                        bool isSuccess = _taskService.DeleteAssignTask(_userService.CurrentUser.Id, taskId);
                        if (isSuccess)
                        {
                            ConsoleWriteSuccessfullyDeletedTask(taskId);
                        }
                        else
                        {
                            ConsoleWriteWasntSuccessfullyDeletedTask(taskId);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"Task with id {taskId} was not found!");
                }
            }
            else
            {
                ToDoListNotExist(toDoListId);
            }
        }

        private static void ConsoleWriteWasntSuccessfullyDeletedTask(int taskId)
        {
            Console.WriteLine($"Task with id {taskId} wasn't successfully deleted!");
        }

        private static void ConsoleWriteSuccessfullyDeletedTask(int taskId)
        {
            Console.WriteLine($"Task with id {taskId} was successfully deleted!");
        }

        private static void CreateTask()
        {
            Console.WriteLine("Write the id of the ToDo list");
            int toDoListId = ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, _userService.CurrentUser.Id);
            bool isExistInSharedCollection = _toDoListService.CheckIfToDoListExistInSharedListsOfUser(toDoListId, _userService.CurrentUser.Id);
            if (isExist || isExistInSharedCollection)
            {
                string title, description; bool isComplete;
                ReadingParametersFromConsole(out title, out description, out isComplete);

                bool isSuccess = _taskService.CreateTaskInToDoList(toDoListId, title, description, isComplete, _userService.CurrentUser.Id);
                if (isSuccess)
                {
                    ConsoleWriteThatTaskIsCreated(title);
                }
                else
                {
                    ConsoleWriteThatTaskExists(title);
                }
            }
            else
            {
                ToDoListNotExist(toDoListId);
            }
        }

        private static void ConsoleWriteThatTaskExists(string title)
        {
            Console.WriteLine($"Already exists Task with title {title}!");
        }

        private static void ConsoleWriteThatTaskIsCreated(string title)
        {
            Console.WriteLine($"Task with title {title} was successfully created!");
        }

        private static void ReadingParametersFromConsole(out string title, out string description, out bool isComplete)
        {
            Console.WriteLine("Write the title of your Task");
            title = Console.ReadLine();
            title = CheckIfTitleIsCorrect(title);
            Console.WriteLine("Write the description of your Task");
            description = Console.ReadLine();
            description = CheckIfTitleIsCorrect(description);
            Console.WriteLine("Is your Task complete or not, write Y or N");
            isComplete = ReadingBooleanFromConsole();
        }

        private static bool ReadingBooleanFromConsole()
        {
            string result = Console.ReadLine();

            while (result != "Y" && result != "N" && result != "y" && result != "n")
            {
                Console.WriteLine("Write Y or N");
                result = Console.ReadLine();
            }
            if (result == "Y" || result == "y")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void ListAllToDoLists()
        {
            List<ToDoList> createdToDoLists = _toDoListService.GetAllListsCreatedByUser(_userService.CurrentUser.Id);
            List<int> allIds = _toDoListService.GetAllIdsOfSharedToDoListsOfTheUser(_userService.CurrentUser.Id);


            if (createdToDoLists.Any())
            {
                Console.WriteLine($"Next lines are all To Do Lists created by user with id {_userService.CurrentUser.Id}");
                foreach (ToDoList toDoList in createdToDoLists)
                {
                    Console.WriteLine($"========================={toDoList.Id}===========================");
                    Console.WriteLine($"ToDo list title is {toDoList.Title}");
                    Console.WriteLine($"The ToDo list was created on {toDoList.DateOfCreation.ToString("D", CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"The last change of the ToDo list was on {toDoList.DateOfLastChange.ToString("D", CultureInfo.InvariantCulture)} and was done by user with id {toDoList.IdOfUserLastChange}");
                    Console.WriteLine($"{toDoList.Tasks.Count} Tasks");
                    Console.WriteLine("=======================================================");

                }
            }
            else
            {
                Console.WriteLine("=======================================================");
                Console.WriteLine($"No lists created by user with id {_userService.CurrentUser.Id} was found!");
                Console.WriteLine("=======================================================");

            }

            if (allIds.Count > 0)
            {
                List<ToDoList> sharedToDoLists = _toDoListService.GetAllListsSharedByUser(allIds);
                Console.WriteLine($"Next lines are all To Do Lists shared with user with id {_userService.CurrentUser.Id}");
                foreach (ToDoList toDoList in sharedToDoLists)
                {
                    Console.WriteLine($"========================={toDoList.Id}===========================");
                    Console.WriteLine($"ToDo list title is {toDoList.Title}");
                    Console.WriteLine($"The ToDo list was created on {toDoList.DateOfCreation}");
                    Console.WriteLine($"The last change of the ToDo list was on {toDoList.DateOfLastChange} and was done by user with id {toDoList.IdOfUserLastChange}");
                    Console.WriteLine($"{toDoList.Tasks.Count} Tasks");
                    Console.WriteLine("=======================================================");

                }
            }
            else
            {
                Console.WriteLine("=======================================================");
                Console.WriteLine($"No shared lists for user with id {_userService.CurrentUser.Id} was found!");
                Console.WriteLine("=======================================================");

            }
        }

        private static void EditToDoList()
        {
            Console.WriteLine("Write Id of the ToDo List which want to be edited!");
            int toDoListId = ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheDatabase(toDoListId);
            Console.WriteLine("Write the new title of the ToDo list ");
            string title = Console.ReadLine();
            CheckIfTitleIsCorrect(title);

            bool isSuccess = _toDoListService.EditToDoList(toDoListId, _userService.CurrentUser.Id, title);
            if (isSuccess)
            {
                Console.WriteLine($"You successfully edited the ToDo list with id {toDoListId}");
            }
            else
            {
                ToDoListNotExist(toDoListId);
            }
        }

        /// <summary>
        /// Shares a To Do List with other user
        /// </summary>
        private static void ShareToDoList()
        {
            Console.WriteLine("Write Id of To Do list which you want to be shared");
            int toDoListId = ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, _userService.CurrentUser.Id);

            if (isExist)
            {
                Console.WriteLine("Write Id of the user with whom you want to share the To Do List");
                int userId = ReadAnIntFromTheConsole();
                if (userId == _userService.CurrentUser.Id)
                {
                    Console.WriteLine("You can not share a To Do List with you, created by you!");
                }
                else
                {
                    bool isUserExist = _userService.CheckIfUserExistById(userId);
                    if (isUserExist)
                    {
                        bool isSuccess = _toDoListService.ShareToDoList(toDoListId, userId);
                        if (isSuccess)
                        {
                            Console.WriteLine($"You successfully shared To Do list with id {toDoListId} with user with id {userId}!");
                        }
                        else
                        {
                            Console.WriteLine($"You already shared To Do list with id {toDoListId} with user with id {userId}!");
                        }

                    }
                    else
                    {
                        Console.WriteLine($"User with id {userId} not exist");
                    }
                }
            }
            else
            {
                ToDoListNotExist(toDoListId);
            }
        }

        private static void DeleteToDoList()
        {
            Console.WriteLine("Write Id of the ToDo List which you want to be deleted!");
            int toDoListId = ReadAnIntFromTheConsole();

            bool isSuccess = _toDoListService.DeleteToDolist(toDoListId, _userService.CurrentUser.Id);
            if (isSuccess)
            {
                Console.WriteLine($"ToDo List with id {toDoListId} was successfully deleted!");
            }
            else
            {
                ToDoListNotExist(toDoListId);
            }
        }

        private static void CreateToDoList()
        {
            Console.WriteLine("Write the title of your ToDo list");
            string title = Console.ReadLine();
            title = CheckIfTitleIsCorrect(title);

            bool isSuccess = _toDoListService.CreateToDoList(title, _userService.CurrentUser.Id);
            if (isSuccess)
            {
                Console.WriteLine($"ToDo List with title {title} was successfully created!");
            }
            else
            {
                Console.WriteLine($"Already exists ToDo List with title {title}!");
            }
        }

        private static string CheckIfTitleIsCorrect(string title)
        {
            while (title == "")
            {
                Console.WriteLine("Please enter a title!");
                title = Console.ReadLine();
            }

            return title;
        }

        private static void ListAllUsers()
        {
            List<User> allUsers = _userService.GetAllUsers();
            if (allUsers.Any())
            {
                foreach (User user in allUsers)
                {
                    Console.WriteLine($"========================={user.Id}===========================");
                    Console.WriteLine($"{user.Username}");
                    Console.WriteLine($"{user.FirstName}");
                    Console.WriteLine($"{user.LastName}");
                    Console.WriteLine($"{user.Role}");
                    Console.WriteLine($"User with id {user.CreatorId} created the user");
                    Console.WriteLine($"The user was created on {user.DateOfCreation.ToString("D", CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"The last change of the user was on {user.DateOfLastChange.ToString("D", CultureInfo.InvariantCulture)} and was done by user with id {user.IdOfUserLastChange}");
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

        private static void DeleteUser()
        {
            Console.WriteLine("Write Id of the user who want to be deleted!");
            int userId = ReadAnIntFromTheConsole();
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

        private static int ReadAnIntFromTheConsole()
        {
            int number;
            bool result;
            result = int.TryParse(Console.ReadLine(), out number);
            while (!result)
            {
                Console.WriteLine("Write a number");
                result = int.TryParse(Console.ReadLine(), out number);
            }
            return number;
        }

        private static void EditUser()
        {
            Console.WriteLine("Write Id of the user who want to be edited!");
            int userId = ReadAnIntFromTheConsole();
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

        private static void CreateUser()
        {
            string username, password, firstname, lastname;
            AskForInformationAboutTheUser(out username, out password, out firstname, out lastname);
            Console.WriteLine("Choose role for the user, write 1 for administrative privileges OR 2 for without administrative privileges");
            int role = ReadAnIntFromTheConsole();
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

        private static int CheckIfParameterIs1Or2(int parameter)
        {
            while (parameter != 1 && parameter != 2)
            {
                Console.WriteLine("Write 1 or 2, please!");
                parameter = ReadAnIntFromTheConsole();
            }

            return parameter;
        }

        private static void Logout()
        {
            _userService.Logout();
        }

        private static void Login()
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
