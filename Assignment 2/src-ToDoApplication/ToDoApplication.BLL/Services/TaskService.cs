using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.BLL.Services
{
    /// <summary>
    /// Manages Task related functionality
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly TaskDatabase _taskDatabase;
        public TaskService(TaskDatabase taskDatabase)
        {
            _taskDatabase = taskDatabase;
        }

        /// <summary>
        /// Creates new Task in ToDo List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isComplete"></param>
        /// <param name="creatorId"></param>
        /// <returns>True if list is created otherwise false</returns>
        public bool CreateTaskInToDoList(int toDoListId, string title, string description, bool isComplete, int creatorId)
        {

            if (_taskDatabase.CheckIfThereIsAlreadyTaskWithGivenTitle(title))
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
                DateOfLastChange = timeOfCreation,
                IdOfUserLastChange = creatorId,
            };
            return _taskDatabase.CreateTask(task);
        }

        /// <summary>
        /// Delete task
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="taskId"></param>
        /// <returns>True if task is deleted otherwise false</returns>
        public bool DeleteTask(int toDoListId, int taskId)
        {
            bool isExist = CheckIfTaskExistInDatabaseById(taskId);
            if (!isExist)
            {
                return false;
            }

            return _taskDatabase.DeleteTask(toDoListId, taskId);
        }

        /// <summary>
        /// Delete Assign Task
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns>True if assigned task is deleted otherwise false</returns>
        public bool DeleteAssignTask(int userId, int taskId)
        {
            return _taskDatabase.DeleteAssignTask(userId, taskId);
        }

        /// <summary>
        /// Edit Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <param name="title"></param>
        /// <param name="description"></param>
        /// <param name="isComplete"></param>
        /// <param name="userId"></param>
        /// <returns>True if task is edited otherwise false</returns>
        public bool EditTask(int taskId, string title, string description, bool isComplete, int userId)
        {
            DateTime dateOfChange = DateTime.Now;
            return _taskDatabase.EditTask(taskId, title, description, isComplete, dateOfChange, userId);
        }

        /// <summary>
        /// Assign Task to a User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns>True if task is assigned deleted otherwise false</returns>
        public bool AssignTask(int userId, int taskId)
        {
            if (_taskDatabase.CheckIfTaskIsAlreadyAssignToUser(userId, taskId))
            {
                return false;
            }
            return _taskDatabase.AssignTask(userId, taskId);
        }

        /// <summary>
        /// Complete Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>True if task is completed otherwise false</returns>
        public bool CompleteTask(int taskId)
        {
            return _taskDatabase.CompleteTask(taskId);
        }

        /// <summary>
        /// Get All Task Of ToDo List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <returns></returns>
        public List<Task> GetAllTasks(int toDoListId)
        {
            return _taskDatabase.GetAllTasks(toDoListId);
        }

        public bool CheckIfTaskExistInDatabaseById(int taskId)
        {
            return _taskDatabase.CheckIfTaskExistInDatabase(taskId);
        }

        public bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId)
        {
            return _taskDatabase.CheckIfUserIsCreatorOfTheTask(taskId, userId);
        }              
    }
}
