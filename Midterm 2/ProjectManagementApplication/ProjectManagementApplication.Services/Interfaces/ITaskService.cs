using ProjectManagementApplication.Services.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = ProjectManagementApplication.Data.Entities.Task;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface ITaskService
    {
        Task<Tuple<Messages,int>> CreateTask(string name, int status, int projectId, string userId);

        Task<bool> DeleteTask(int taskId, int projectId, string userId);

        Task<bool> EditTask(int taskId, string name, int status, string assigneeId, string userId);

        Task<bool> AssignTask(int taskId, string assigneeId, string userId);

        Task<List<Task>> GetAllTasks(int projectId, string userId);

        Task<Task> GetTaskById(int taskId, string userId);
    }
}
