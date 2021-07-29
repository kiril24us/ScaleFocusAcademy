using System.Collections.Generic;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Repositories
{
    public interface ITaskRepository : IRepository<Entities.Task>
    {
        bool AssignTask(UserTask assignedTask);

        bool CheckIfThereIsAlreadyTaskWithGivenTitle(string title);

        bool CheckIfTaskExistInDatabaseById(int taskId);

        bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId);

        bool CompleteTask(int taskId);

        bool CheckIfThereIsAssignedTaskToTheUser(int userId, int taskId);

        Task GetTaskById(int taskId);

        List<int> GetAllTaskIdsCreatedToToDoList(int toDoListId);

        UserTask GetSharedTaskById(int userId, int taskId);

        List<Task> GetAllTasks(int toDoListId);

        bool DeleteAssignTask(UserTask sharedTaskToRemove);

        bool CheckIfTaskIsAlreadyAssignToUser(int userId, int taskId);
    }
}
