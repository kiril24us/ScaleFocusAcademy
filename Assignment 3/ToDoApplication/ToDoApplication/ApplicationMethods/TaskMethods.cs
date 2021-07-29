using System;
using ToDoApplication.DAL.Entities;
using ToDoApplication.BLL.Services;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;

namespace ToDoApplication.ApplicationMethods
{
    public class TaskMethods
    {
        private static IToDoListService _toDoListService;
        private static IUserService _userService;
        private static ITaskService _taskService;

        public TaskMethods(ITaskService taskService, IToDoListService toDoListService, IUserService userService)
        {
            _taskService = taskService;
            _toDoListService = toDoListService;
            _userService = userService;
        }

        public void CreateTask()
        {
            Console.WriteLine("Write the id of the ToDo list");
            int toDoListId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, _userService.CurrentUser.UserId);
            bool isExistInSharedCollection = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, _userService.CurrentUser.UserId);
            if (isExist || isExistInSharedCollection)
            {
                string title, description; bool isComplete;
                ReadingParametersFromConsole(out title, out description, out isComplete);

                bool isSuccess = _taskService.CreateTaskInToDoList(toDoListId, title, description, isComplete, _userService.CurrentUser.UserId);
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
                CommonMethods.ToDoListNotExist(toDoListId);
            }
        }

        public void CompleteTask()
        {
            Console.WriteLine("Write the Task Id which you want to complete");
            int taskId = CommonMethods.ReadAnIntFromTheConsole();

            bool isExist = _taskService.CheckIfTaskExistInDatabaseById(taskId);
            if (isExist)
            {
                int toDoListId = _toDoListService.GetToDoListIdWhichContainsTask(taskId);
                if (toDoListId > 0)
                {
                    bool isExistToDoListInUser = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, _userService.CurrentUser.UserId);
                    bool isExistSharedToDoListInUser = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, _userService.CurrentUser.UserId);
                    if (isExistToDoListInUser || isExistSharedToDoListInUser)
                    {
                        bool isSuccess = _taskService.CompleteTask(taskId);
                        if (isSuccess)
                        {
                            Console.WriteLine($"You successfully completed task with id {taskId} ");
                        }
                        else
                        {
                            Console.WriteLine($"You don't successfully completed task with id {taskId} ");
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine($"No Tasks with id {taskId} was found");
            }
        }

        public void AssignTask()
        {
            Console.WriteLine("Write Id of the Task which you want to be assigned");
            int taskId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _taskService.CheckIfTaskExistInDatabaseById(taskId);

            if (isExist)
            {
                Console.WriteLine($"Write id of the user to assign task with id {taskId}");
                int userId = CommonMethods.ReadAnIntFromTheConsole();
                bool isExistUser = _userService.CheckIfUserExistById(userId);
                if (isExistUser)
                {
                    int toDoListId = _toDoListService.GetToDoListIdWhichContainsTask(taskId);
                    if (toDoListId > 0)
                    {
                        bool isExistToDoListInUser = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, userId);
                        bool isExistSharedToDoListInUser = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, userId);
                        if (isExistToDoListInUser || isExistSharedToDoListInUser)
                        {
                            Task task = _taskService.GetTaskById(taskId);
                            if (task.CreatedById == userId)
                            {
                                Console.WriteLine("You cannot assign a task to yourself! ");
                            }
                            else
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
                        }
                        else
                        {
                            Console.WriteLine($"User with id {userId} doesn't have ToDo List with id {toDoListId} ");
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"User with id {userId} was not found ");
                }
            }
            else
            {
                Console.WriteLine($"Task with id {taskId} was not found ");
            }
        }

        public void ListAllTasks()
        {
            Console.WriteLine("Write Id of the ToDo List from which you want to list all Tasks!");
            int toDoListId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExistToDoList = _toDoListService.CheckIfToDoListExistInTheDatabase(toDoListId);
            if (isExistToDoList)
            {
                bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, _userService.CurrentUser.UserId);
                bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, _userService.CurrentUser.UserId);
                if (isExist || isExistInSharedToDoListsCollection)
                {
                    List<Task> tasks = _taskService.GetAllTasks(toDoListId);
                    PrintInfoForTasks(toDoListId, tasks);
                }
                else
                {
                    Console.WriteLine($"ToDo List with id {toDoListId} exists in the database but is not shared with you or created by you");
                }
            }
            else
            {
                CommonMethods.ToDoListNotExist(toDoListId);
            }
        }

        private static void PrintInfoForTasks(int toDoListId, List<Task> tasks)
        {
            if (tasks.Any())
            {
                Console.WriteLine($"Next lines are all Tasks in To Do List with id {toDoListId}");
                foreach (Task task in tasks)
                {
                    Console.WriteLine($"========================={task.TaskId}===========================");
                    Console.WriteLine($"Task title is {task.Title}");
                    Console.WriteLine($"Description: {task.Description}");
                    Console.WriteLine($"Task is complete - {task.IsComplete}");
                    Console.WriteLine($"Task was created on {task.CreatedOn.ToString("D", CultureInfo.InvariantCulture)}");
                    Console.WriteLine($"The last change of the Task was on {task.LastModifiedOn.ToString("D", CultureInfo.InvariantCulture)} and was done by user with id {task.LastModifiedById}");
                    Console.WriteLine($"Creator of the Task is User with id - {task.CreatedById}");
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

        public void EditTask()
        {
            Console.WriteLine("Write Id of the ToDo List from which you want to edit a Task!");
            int toDoListId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, _userService.CurrentUser.UserId);
            bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, _userService.CurrentUser.UserId);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                Console.WriteLine("Write Id of the Task which you want to be edited!");
                int taskId = CommonMethods.ReadAnIntFromTheConsole();
                if (_taskService.CheckIfTaskExistInDatabaseById(taskId))
                {
                    Console.WriteLine("Write new title for the Task");
                    string title = Console.ReadLine();
                    title = CommonMethods.CheckIfTitleIsCorrect(title);
                    Console.WriteLine("Write new description for the Task");
                    string description = Console.ReadLine();
                    description = CommonMethods.CheckIfTitleIsCorrect(description);
                    Console.WriteLine("Is your Task complete or not, write Y or N");
                    bool isComplete = CommonMethods.ReadingBooleanFromConsole();
                    bool isSuccess = _taskService.EditTask(taskId, title, description, isComplete, _userService.CurrentUser.UserId);
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
               CommonMethods.ToDoListNotExist(toDoListId);
            }
        }

        public void DeleteTask()
        {
            Console.WriteLine("Write Id of the ToDo List from which you want to delete a Task!");
            int toDoListId = CommonMethods.ReadAnIntFromTheConsole();
            bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, _userService.CurrentUser.UserId);
            bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, _userService.CurrentUser.UserId);
            if (isExist || isExistInSharedToDoListsCollection)
            {
                Console.WriteLine("Write Id of the Task which you want to be deleted!");
                int taskId = CommonMethods.ReadAnIntFromTheConsole();
                bool isExistTask = _taskService.CheckIfTaskExistInDatabaseById(taskId);
                if (isExistTask)
                {
                    bool isCreator = _taskService.CheckIfUserIsCreatorOfTheTask(taskId, _userService.CurrentUser.UserId);
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
                        bool isSuccess = _taskService.DeleteAssignTask(_userService.CurrentUser.UserId, taskId);
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
                CommonMethods.ToDoListNotExist(toDoListId);
            }
        }

        private static void ConsoleWriteSuccessfullyAssignTask(int taskId, int userId)
        {
            Console.WriteLine($"You successfully assign task with id {taskId} to user with id {userId}");
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
            title = CommonMethods.CheckIfTitleIsCorrect(title);
            Console.WriteLine("Write the description of your Task");
            description = Console.ReadLine();
            description = CommonMethods.CheckIfTitleIsCorrect(description);
            Console.WriteLine("Is your Task complete or not, write Y or N");
            isComplete = CommonMethods.ReadingBooleanFromConsole();
        }

        private static void ConsoleWriteWasntSuccessfullyDeletedTask(int taskId)
        {
            Console.WriteLine($"Task with id {taskId} wasn't successfully deleted!");
        }

        private static void ConsoleWriteSuccessfullyDeletedTask(int taskId)
        {
            Console.WriteLine($"Task with id {taskId} was successfully deleted!");
        }
    }
}
