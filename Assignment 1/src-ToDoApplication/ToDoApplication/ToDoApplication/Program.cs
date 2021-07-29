using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApplication.Entities;
using ToDoApplication.Services;

namespace ToDoApplication
{
    class Program
    {
        private static readonly IUserService _userService = new UserService();
        private static readonly IToDoListService _toDoListService = new ToDoListService();
        private static readonly ITaskService _taskService = new TaskService();

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                _userService.Login(args[0], args[1]);
            }

            bool shouldExit = false;
            while (!shouldExit)
            {
                shouldExit = MainMenu();
            }
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
                        Logout();
                    }
                    return false;
                case "2":
                    ListAllUsers();
                    return false;
                case "3":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        AddUser();
                    }
                    else
                    {
                        CreateToDoList();
                    }
                    return false;
                case "4":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        EditUser();
                    }
                    else
                    {
                        DeleteToDoList();
                    }
                    break;
                case "5":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        DeleteUser();
                    }
                    else
                    {
                        ShareToDoList();
                    }
                    return false;
                case "6":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        CreateToDoList();
                    }
                    else
                    {
                        EditToDoList();
                    }
                    return false;
                case "7":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        DeleteToDoList();
                    }
                    else
                    {
                        ListAllToDoLists();
                    }
                    return false;
                case "8":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        ShareToDoList();
                    }
                    else
                    {
                        CreateTask();
                    }
                    return false;
                case "9":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        EditToDoList();
                    }
                    else
                    {
                        DeleteTask();
                    }
                    return false;
                case "10":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        ListAllToDoLists();
                    }
                    else
                    {
                        EditTask();
                    }
                    return false;
                case "11":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        CreateTask();
                    }
                    else
                    {
                        ListAllTasks();
                    }
                    return false;
                case "12":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        DeleteTask();
                    }
                    else
                    {
                        AssignTask();
                    }
                    return false;
                case "13":
                    if (_userService.CurrentUser.Role.ToString() == "Admin")
                    {
                        EditTask();
                    }
                    else
                    {
                        CompleteTask();
                    }
                    return false;
                case "14":
                    ListAllTasks();
                    return false;
                case "15":
                    AssignTask();
                    return false;
                case "16":
                    CompleteTask();
                    return false;
                case "exit":
                    return true;
                default:
                    Console.WriteLine("Unknown commands!");
                    return false;
            }
            return false;
        }

        private static void CompleteTask()
        {
            Console.WriteLine("Write the Task Id which you want to complete");
            int taskId = ReadAnIntFromTheConsole();

            List<Task> tasksToComplete = _taskService.FindAllTasksById(taskId);
            if (tasksToComplete.Any())
            {
                _taskService.CompleteTasks(tasksToComplete);
                Console.WriteLine($"You successfully completed all tasks with id {taskId} ");
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
            bool isExist = _taskService.CheckIfTaskExistsInAllToDoLists(taskId);
            bool isExistInSharedToDoLists = _taskService.CheckIfTaskExistsInAllSharedToDoLists(taskId);
            if (isExist || isExistInSharedToDoLists)
            {
                Console.WriteLine($"Write id of the user to assign task with id {taskId}");
                int userId = ReadAnIntFromTheConsole();
                ToDoList toDoList = _toDoListService.FindToDoListWhichContainsTask(taskId);
                if(toDoList != null)
                {
                    bool isInToDoList = _toDoListService.CheckIfToDoListExistInTheUser(toDoList.Id, userId);
                    if (isInToDoList)
                    {
                        _taskService.AssignTask(taskId, userId, toDoList);
                        ConsoleWriteSuccessfullyAssignTask(taskId, userId);
                    }
                    else
                    {
                        _taskService.AssignTaskInSharedToDoList(taskId, userId, toDoList);
                        ConsoleWriteSuccessfullyAssignTask(taskId, userId);
                    }
                }
                
                else
                {
                    ToDoList sharedToDoList = _toDoListService.FindSharedToDoListWhichContainsTask(taskId);
                    bool isInSharedToDoList = _toDoListService.CheckIfSharedToDoListExistInTheUser(sharedToDoList.Id, userId);
                    if (!isInSharedToDoList)
                    {
                        _taskService.AssignTask(taskId, userId, sharedToDoList);
                        ConsoleWriteSuccessfullyAssignTask(taskId, userId);
                    }
                    else
                    {
                        _taskService.AssignTaskInSharedToDoList(taskId, userId, sharedToDoList);
                        ConsoleWriteSuccessfullyAssignTask(taskId, userId);
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
            bool isExistInSharedToDoListsCollection = _toDoListService.IsExistInSharedToDoListInTheUser(toDoListId, _userService.CurrentUser.Id);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                if (isExist)
                {
                    List<Task> tasks = _taskService.GetAllTasksFromToDoListCreatedByUser(toDoListId, _userService.CurrentUser.Id);
                    PrintInfoForTasks(toDoListId, tasks);
                }
                else
                {
                    List<Task> tasksFromSharedToDoList = _taskService.GetAllTasksFromSharedToDoListWithUser(toDoListId, _userService.CurrentUser.Id);
                    PrintInfoForTasks(toDoListId, tasksFromSharedToDoList);
                }
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
                    Console.WriteLine($"Task was created on {task.DateOfCreation}");
                    Console.WriteLine($"The last change of the Task was on {task.DateOfLastChange} and was done by user with id {task.IdOfUserLastChange}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Task is complete - {task.IsComplete}");
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
            bool isExist = _taskService.CheckIfToDoListExistInAllUsersToDoLists(toDoListId);
            bool isExistInSharedToDoListsCollection = _taskService.CheckIfToDoListExistInAllUsersSharedToDoLists(toDoListId);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                Console.WriteLine("Write Id of the Task which you want to be edited!");
                int taskId = ReadAnIntFromTheConsole();
                Console.WriteLine("Write new title for the Task");
                string title = Console.ReadLine();
                title = CheckIfTitleIsCorrect(title);
                Console.WriteLine("Write new description for the Task");
                string description = Console.ReadLine();
                description = CheckIfTitleIsCorrect(description);
                Console.WriteLine("Is your Task complete or not, write true or false");
                bool isComplete = ReadingBooleanFromConsole();
                bool isSuccess = _taskService.EditTask(toDoListId, taskId, _userService.CurrentUser.Id, title, description, isComplete);
                if (isSuccess)
                {
                    Console.WriteLine($"Task with id {taskId} was successfully edited!");
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
            bool isExistInSharedToDoListsCollection = _toDoListService.IsExistInSharedToDoListInTheUser(toDoListId, _userService.CurrentUser.Id);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                Console.WriteLine("Write Id of the Task which you want to be deleted!");
                int taskId = ReadAnIntFromTheConsole();
                bool isSuccess = _taskService.DeleteTask(toDoListId, taskId, _userService.CurrentUser.Id);
                if (isSuccess)
                {
                    Console.WriteLine($"Task with id {taskId} was successfully deleted!");
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

        private static void CreateTask()
        {
            Console.WriteLine("Write the id of the ToDo list");
            int toDoListId = ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, _userService.CurrentUser.Id);
            bool isExistInSharedToDoListsCollection = _toDoListService.IsExistInSharedToDoListInTheUser(toDoListId, _userService.CurrentUser.Id);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                string title, description; bool isComplete;
                ReadingParametersFromConsole(out title, out description, out isComplete);
                if (isExist)
                {
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
                    bool isSuccess = _taskService.CreateTaskInSharedToDoList(toDoListId, title, description, isComplete, _userService.CurrentUser.Id);
                    if (isSuccess)
                    {
                        ConsoleWriteThatTaskIsCreated(title);
                    }
                    else
                    {
                        ConsoleWriteThatTaskExists(title);
                    }
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
            Console.WriteLine("Is your Task complete or not, write true or false");
            isComplete = ReadingBooleanFromConsole();
        }

        private static bool ReadingBooleanFromConsole()
        {
            string result = Console.ReadLine();

            while (result != "true" && result != "false")
            {
                Console.WriteLine("Write true or false");
                result = Console.ReadLine();
            }
            if (result == "true")
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
            List<ToDoList> sharedToDoLists = _toDoListService.GetAllListsSharedByUser(_userService.CurrentUser.Id);

            if (createdToDoLists.Any())
            {
                Console.WriteLine($"Next lines are all To Do Lists created by user with id {_userService.CurrentUser.Id}");
                foreach (ToDoList toDoList in createdToDoLists)
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
                Console.WriteLine($"No lists created by user with id {_userService.CurrentUser.Id} was found!");
                Console.WriteLine("=======================================================");

            }

            if (sharedToDoLists.Any())
            {
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
                    bool isUserExist = _userService.CheckIfUserExistInTheStorage(userId);
                    if (isUserExist)
                    {
                        bool isSuccess = _toDoListService.ShareToDoList(toDoListId, userId, _userService.CurrentUser.Id);
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
                    Console.WriteLine($"The user was created on {user.DateOfCreation}");
                    Console.WriteLine($"The last change of the user was on {user.DateOfLastChange} and was done by user with id {user.IdOfUserLastChange}");
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
            bool isExist = _userService.CheckIfUserExistInTheStorage(userId);
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

        private static void AddUser()
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
                Console.WriteLine("1. LogOut ");

            }
            if (_userService.CurrentUser.Role.ToString() == "Admin")
            {
                Console.WriteLine("2. List all Users ");
                Console.WriteLine("3. Create User ");
                Console.WriteLine("4. Edit User ");
                Console.WriteLine("5. Delete User ");
                Console.WriteLine("6. Create ToDo list ");
                Console.WriteLine("7. Delete ToDo list ");
                Console.WriteLine("8. Share ToDo list with other user ");
                Console.WriteLine("9. Edit ToDo list ");
                Console.WriteLine("10. List all ToDo lists ");
                Console.WriteLine("11. Create a Task ");
                Console.WriteLine("12. Delete a Task ");
                Console.WriteLine("13. Edit a Task ");
                Console.WriteLine("14. List all Tasks in a single To Do List ");
                Console.WriteLine("15. Assign a Task ");
                Console.WriteLine("16. Complete a Task ");
            }
            else
            {
                Console.WriteLine("2. List all Users ");
                Console.WriteLine("3. Create ToDo list ");
                Console.WriteLine("4. Delete ToDo list ");
                Console.WriteLine("5. Share ToDo list with other user ");
                Console.WriteLine("6. Edit ToDo list ");
                Console.WriteLine("7. List all ToDo lists ");
                Console.WriteLine("8. Create a Task ");
                Console.WriteLine("9. Delete a Task ");
                Console.WriteLine("10. Edit a Task ");
                Console.WriteLine("11. List all Tasks in a single To Do List ");
                Console.WriteLine("12. Assign a Task ");
                Console.WriteLine("13. Complete a Task ");
            }
        }

    }
}
