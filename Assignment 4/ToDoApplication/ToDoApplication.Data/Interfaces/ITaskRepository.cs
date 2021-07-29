using System.Collections.Generic;
using ToDoApplication.Data.Entities;

namespace ToDoApplication.Data.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        bool AssignTask(UserTask assignedTask);

        bool CheckIfThereIsAlreadyTaskWithGivenTitle(string title);

        bool CheckIfTaskExistInDatabaseById(int taskId);

        bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId);

        bool CompleteTask(int taskId);

        Task GetTaskById(int taskId);

        Task GetTaskByTitle(string title);

        UserTask GetSharedTaskById(int userId, int taskId);

        bool DeleteAssignTask(UserTask sharedTaskToRemove);

        bool CheckIfTaskIsAlreadyAssignToUser(int userId, int taskId);       
    }
}
