using ProjectManagementApplication.Services.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = ProjectManagementApplication.Data.Entities.Task;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface ITaskService
    {
        Task<Tuple<Messages,int>> CreateTask(string name, int status, int projectId, int userId);

        Task<bool> DeleteTask(int taskId, int projectId, int userId);

        Task<bool> EditTask(int taskId, string name, int status, int assigneeId, int userId);

        Task<bool> AssignTask(int taskId, int assigneeId, int userId);

        Task<List<Task>> GetAllTasks(int projectId, int userId);

        Task<Task> GetTaskById(int taskId, int userId);
    }
}
