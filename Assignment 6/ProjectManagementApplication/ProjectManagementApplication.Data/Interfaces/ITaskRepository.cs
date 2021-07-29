using System.Collections.Generic;
using System.Threading.Tasks;
using Task = ProjectManagementApplication.Data.Entities.Task;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface ITaskRepository : IRepository<Task>
    {
        Task<bool> Delete(int taskId, int projectId, int userId);

        Task<Task> GetTaskByName(string name);

        Task<Task> GetTaskById(int taskId, int userId);

        Task<List<Task>> GetAllTasks(int projectId, int userId);
    }
}
