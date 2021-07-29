using System.Collections.Generic;
using ToDoApplication.Data.Entities;

namespace ToDoApplication.Services.Interfaces
{
    public interface ITaskService
    {
        bool CreateTaskInToDoList(int toDoListId, string title, string description, bool isComplete, int creatorId);

        bool DeleteTask(int toDoListId, int taskId);

        bool DeleteAssignTask(int userId, int taskId);

        bool EditTask(int taskId, string title, string description, bool isComplete, int userId);

        List<Task> GetAllTasks(int toDoListId);

        Task GetTaskById(int taskId);

        Task GetTaskByTitle(string title);

        bool AssignTask(int userId, int taskId);

        bool CompleteTask(int taskId);

        bool CheckIfTaskExistInDatabaseById(int taskId);

        bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId);
    }
}
