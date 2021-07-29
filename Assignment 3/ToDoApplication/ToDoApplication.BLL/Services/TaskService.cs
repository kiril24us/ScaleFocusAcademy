using System;
using System.Collections.Generic;
using ToDoApplication.DAL.Entities;
using ToDoApplication.DAL.Repositories;

namespace ToDoApplication.BLL.Services
{
    /// <summary>
    /// Manages Task related functionality
    /// </summary>
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {           
            _taskRepository = taskRepository;
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

            if (_taskRepository.CheckIfThereIsAlreadyTaskWithGivenTitle(title))
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
                CreatedById = creatorId,
                CreatedOn = timeOfCreation,
                LastModifiedOn = timeOfCreation,
                LastModifiedById = creatorId,
            };
            return _taskRepository.Create(task);
        }

        /// <summary>
        /// Delete task
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="taskId"></param>
        /// <returns>True if task is deleted otherwise false</returns>
        public bool DeleteTask(int toDoListId, int taskId)
        {
            Task taskToRemove = _taskRepository.GetTaskById(taskId);
            return _taskRepository.Delete(taskToRemove);
        }

        /// <summary>
        /// Delete Assign Task
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns>True if assigned task is deleted otherwise false</returns>
        public bool DeleteAssignTask(int userId, int taskId)
        {
            UserTask sharedTaskToRemove = _taskRepository.GetSharedTaskById(userId, taskId);
            return _taskRepository.DeleteAssignTask(sharedTaskToRemove);
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

            Task taskToBeEdited = _taskRepository.GetTaskById(taskId);
            taskToBeEdited.Title = title;
            taskToBeEdited.Description = description;
            taskToBeEdited.IsComplete = isComplete;
            taskToBeEdited.LastModifiedById = userId;
            taskToBeEdited.LastModifiedOn = dateOfChange;

            return _taskRepository.Edit();
        }

        /// <summary>
        /// Assign Task to a User
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="taskId"></param>
        /// <returns>True if task is assigned deleted otherwise false</returns>
        public bool AssignTask(int userId, int taskId)
        {
            if (_taskRepository.CheckIfTaskIsAlreadyAssignToUser(userId, taskId))
            {
                return false;
            }

            UserTask assignedTask = new UserTask
            {
                UserId = userId,
                TaskId = taskId
            };
            return _taskRepository.AssignTask(assignedTask);
        }

        /// <summary>
        /// Complete Task
        /// </summary>
        /// <param name="taskId"></param>
        /// <returns>True if task is completed otherwise false</returns>
        public bool CompleteTask(int taskId)
        {
            return _taskRepository.CompleteTask(taskId);
        }

        /// <summary>
        /// Get All Task Of ToDo List
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <returns></returns>
        public List<Task> GetAllTasks(int toDoListId)
        {
            return _taskRepository.GetAllTasks(toDoListId);
        }

        public bool CheckIfTaskExistInDatabaseById(int taskId)
        {
            return _taskRepository.CheckIfTaskExistInDatabaseById(taskId);
        }

        public bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId)
        {
            return _taskRepository.CheckIfUserIsCreatorOfTheTask(taskId, userId);
        }

        public Task GetTaskById(int taskId)
        {
            return _taskRepository.GetTaskById(taskId);
        }
    }
}
