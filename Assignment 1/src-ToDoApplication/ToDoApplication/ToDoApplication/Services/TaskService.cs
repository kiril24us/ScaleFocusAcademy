using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ToDoApplication.Data;
using ToDoApplication.Entities;

namespace ToDoApplication.Services
{
    /// <summary>
    /// Manages Task related functionality
    /// </summary>
    public class TaskService : ITaskService
    {
        private const string StoreFileNameForTasks = "Users.json";
        private const string StoreFileNameForCounterTasks = "CounterTasks.json";
        private int counterForCreateTasks;
        private readonly FileDatabase _storage;
        private static readonly IUserService _userService = new UserService();
        private static readonly IToDoListService _toDoListService = new ToDoListService();

        public TaskService()
        {
            _storage = new FileDatabase();
            int counterTasksFromFile = _storage.ReadNumber(StoreFileNameForCounterTasks);
            counterForCreateTasks = counterTasksFromFile;
        }

        /// <summary>
        /// Creates new Task in To Do List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isComplete"></param>
        /// <param name="creatorId"></param>
        /// <returns>True if list is created otherwise false</returns>
        public bool CreateTaskInToDoList(int toDoListId, string title, string description, bool isComplete, int creatorId)
        {
            User user = _userService.FindUserById(creatorId);
            ToDoList toDoList = user.ToDoLists.FirstOrDefault(x => x.Id == toDoListId);

            if (toDoList.Tasks.Any(x => x.Title == title))
            {
                return false;
            }
            DateTime timeOfCreation = DateTime.Now;            
            Task task = new Task
            {
                Title = title,
                Description = description,
                IsComplete = isComplete,
                ToDoListId = toDoListId,
                CreatorId = creatorId,
                DateOfCreation = timeOfCreation,
                Id = counterForCreateTasks,
            };
            counterForCreateTasks++;
            toDoList.Tasks.Add(task);
            _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
            _storage.Write(StoreFileNameForCounterTasks, counterForCreateTasks);
            return true;
        }

        /// <summary>
        /// Creates new Task in a Shared To Do List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isComplete"></param>
        /// <param name="creatorId"></param>
        /// <returns>True if list is created otherwise false</returns>
        public bool CreateTaskInSharedToDoList(int toDoListId, string title, string description, bool isComplete, int creatorId)
        {
            User user = _userService.FindUserById(creatorId);
            ToDoList sharedToDoList = user.SharedToDoLists.FirstOrDefault(x => x.Id == toDoListId);

            if (sharedToDoList.Tasks.Any(x => x.Title == title))
            {
                return false;
            }

            DateTime timeOfCreation = DateTime.Now;
            Task task = new Task
            {
                Title = title,
                Description = description,
                IsComplete = isComplete,
                ToDoListId = toDoListId,
                CreatorId = creatorId,
                DateOfCreation = timeOfCreation,
                Id = counterForCreateTasks,
            };

            counterForCreateTasks++;
            sharedToDoList.Tasks.Add(task);
            _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
            _storage.Write(StoreFileNameForCounterTasks, counterForCreateTasks);
            return true;
        }

        public bool DeleteTask(int toDoListId, int taskId, int userId)
        {
            bool isExist = _toDoListService.CheckIfToDoListExistInTheUser(toDoListId, userId);

            if (isExist)
            {
                ToDoList toDoList = _toDoListService.FindToDoListInHisCreator(toDoListId, userId);
                Task task = FindTaskById(toDoList, taskId);
                toDoList.Tasks.Remove(task);
                _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
            }
            else
            {
                ToDoList sharedToDoList = _toDoListService.FindSharedToDoListById(toDoListId, userId);
                Task task = FindTaskById(sharedToDoList, taskId);
                sharedToDoList.Tasks.Remove(task);
                _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
            }

            _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
            return true;
        }

        public Task FindTaskById(ToDoList toDoList, int taskId)
        {
            return toDoList.Tasks.FirstOrDefault(x => x.Id == taskId);
        }

        public bool EditTask(int toDoListId, int taskId, int userId, string title, string description, bool isComplete)
        {
            bool isExist = CheckIfToDoListExistInAllUsersToDoLists(toDoListId);
            DateTime dateOfChange = DateTime.Now;
            if (isExist)
            {
                ToDoList toDoList = UserService._applicationUsers.SelectMany(u => u.ToDoLists).FirstOrDefault(l => l.Id == toDoListId);
                Task task = FindTaskById(toDoList, taskId);
                if (toDoList.Tasks.Contains(task))
                {
                    AssigningNewValuesToATask(userId, title, description, isComplete, dateOfChange, toDoList, task);
                    _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
                    return true;
                }
                return false;
            }
            else
            {
                ToDoList sharedToDoList = UserService._applicationUsers.SelectMany(u => u.SharedToDoLists).FirstOrDefault(l => l.Id == toDoListId);
                Task task = FindTaskById(sharedToDoList, taskId);
                if (sharedToDoList.Tasks.Contains(task))
                {
                    AssigningNewValuesToATask(userId, title, description, isComplete, dateOfChange, sharedToDoList, task);
                    _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
                    return true;
                }
                return false;
            }
        }

        private static void AssigningNewValuesToATask(int userId, string title, string description, bool isComplete, DateTime dateOfChange, ToDoList toDoList, Task task)
        {
            if (toDoList.Tasks.Contains(task))
            {
                task.Title = title;
                task.Description = description;
                task.IsComplete = isComplete;
                task.DateOfLastChange = dateOfChange;
                task.IdOfUserLastChange = userId;
            }
        }

        public bool CheckIfToDoListExistInAllUsersToDoLists(int toDoListId)
        {
            return UserService._applicationUsers.Any(x => x.ToDoLists.Any(x => x.Id == toDoListId));
        }

        public bool CheckIfToDoListExistInAllUsersSharedToDoLists(int toDoListId)
        {
            return UserService._applicationUsers.Any(x => x.SharedToDoLists.Any(x => x.Id == toDoListId));
        }

        public List<Task> GetAllTasksFromToDoListCreatedByUser(int toDoListId, int userId)
        {
            ToDoList toDoList = _toDoListService.FindToDoListInHisCreator(toDoListId, userId);
            return toDoList.Tasks.ToList();
        }

        public List<Task> GetAllTasksFromSharedToDoListWithUser(int toDoListId, int userId)
        {
            ToDoList sharedToDoList = _toDoListService.FindSharedToDoListById(toDoListId, userId);
            return sharedToDoList.Tasks.ToList();
        }

        public bool AssignTask(int taskId, int userId, ToDoList toDoList)
        {
            Task task = FindTaskById(toDoList, taskId);
            User user = _userService.FindUserById(userId);
            if(!user.ToDoLists.SelectMany(x => x.Tasks).Any(x => x.Id == taskId))
            {
                user.ToDoLists.FirstOrDefault(x => x.Id == toDoList.Id).Tasks.Add(task);
            }
            _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
            return true;
        }

        public bool AssignTaskInSharedToDoList(int taskId, int userId, ToDoList toDoList)
        {
            Task task = FindTaskById(toDoList, taskId);
            User user = _userService.FindUserById(userId);
            if (!user.SharedToDoLists.SelectMany(x => x.Tasks).Any(x => x.Id == taskId))
            {
                user.SharedToDoLists.FirstOrDefault(x => x.Id == toDoList.Id).Tasks.Add(task);
            }
            _storage.Write(StoreFileNameForTasks, UserService._applicationUsers);
            return true;
        }

        public List<ToDoList> GetAllListsThatContainsTask(int taskId)
        {
            List<ToDoList> toDoLists = UserService._applicationUsers.SelectMany(x => x.ToDoLists).Where(x => x.Id == taskId).ToList();
            List<ToDoList> sharedToDoLists = UserService._applicationUsers.SelectMany(x => x.ToDoLists).Where(x => x.Id == taskId).ToList();
            List<ToDoList> combinedList = toDoLists.Concat(sharedToDoLists).ToList();
            return combinedList;
        }

        public List<Task> FindAllTasksById(int taskId)
        {
            List<Task> tasksFromToDoLists = UserService._applicationUsers.SelectMany(x => x.ToDoLists).SelectMany(x => x.Tasks).Where(x => x.Id == taskId).ToList();
            List<Task> tasksFromSharedToDoLists = UserService._applicationUsers.SelectMany(x => x.SharedToDoLists).SelectMany(x => x.Tasks).Where(x => x.Id == taskId).ToList();
            List<Task> combinedTasks = tasksFromToDoLists.Concat(tasksFromSharedToDoLists).ToList();
            return combinedTasks;
        }

        public void CompleteTasks(List<Task> tasksToComplete)
        {
            foreach (Task task in tasksToComplete)
            {
                task.IsComplete = true;
            }
        }

        public bool CheckIfTaskExistsInAllToDoLists(int taskId)
        {
            return UserService._applicationUsers.SelectMany(x => x.ToDoLists).SelectMany(x => x.Tasks).Any(x => x.Id == taskId);
        }

        public bool CheckIfTaskExistsInAllSharedToDoLists(int taskId)
        {
            return UserService._applicationUsers.SelectMany(x => x.SharedToDoLists).SelectMany(x => x.Tasks).Any(x => x.Id == taskId);
        }
        
    }
}
