using System.Collections.Generic;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.BLL.Services
{
    public interface ITaskService
    {
        bool CreateTaskInToDoList(int toDoListId, string title, string description, bool isComplete, int creatorId);

        bool DeleteTask(int toDoListId, int taskId);

        bool DeleteAssignTask(int userId, int taskId);

        bool EditTask(int taskId, string title, string description, bool isComplete, int userId);

        List<Task> GetAllTasks(int toDoListId);

        bool AssignTask(int userId, int taskId);

        bool CompleteTask(int taskId);

        bool CheckIfTaskExistInDatabaseById(int taskId);

        bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId);      
    }
}
