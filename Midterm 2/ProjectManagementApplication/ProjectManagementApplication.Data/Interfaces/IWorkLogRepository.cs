using ProjectManagementApplication.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface IWorkLogRepository : IRepository<WorkLog>
    {
        Task<bool> CheckIfTaskIsAssignedToUser(int taskId, string userId);

        Task<WorkLog> GetUserWorkLogByDescription(string description, string userId);

        Task<WorkLog> GetUserWorkLogById(int workLogId, int taskId, string userId);

        Task<List<WorkLog>> GetAll(int taskId);

        Task<bool> Delete(int workLogId, int taskId, string userId);
    }
}
