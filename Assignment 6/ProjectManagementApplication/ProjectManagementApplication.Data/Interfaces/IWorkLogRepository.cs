using ProjectManagementApplication.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface IWorkLogRepository : IRepository<WorkLog>
    {
        Task<bool> CheckIfTaskIsAssignedToUser(int taskId, int userId);

        Task<WorkLog> GetUserWorkLogByDescription(string description, int userId);

        Task<WorkLog> GetUserWorkLogById(int workLogId, int taskId, int userId);

        Task<List<WorkLog>> GetAll(int taskId, int userId);

        Task<bool> Delete(int workLogId, int taskId, int userId);
    }
}
