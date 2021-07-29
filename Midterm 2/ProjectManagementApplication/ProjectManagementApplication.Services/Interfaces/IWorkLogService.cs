using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Services.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface IWorkLogService
    {
        Task<Tuple<Messages, int>> CreateWorkLog(int taskId, string Description, TimeSpan TimeSpent, DateTime startDate, string userId);

        Task<bool> DeleteWorkLog(int workLogId, int taskId, string userId);

        Task<bool> EditWorkLog(int workLogId, string description, int taskId, DateTime startDate, TimeSpan timeSpent, string userId);

        Task<WorkLog> GetUserWorkLogById(int workLogId, int taskId, string userId);

        Task<List<WorkLog>> GetAllWorkLogs(int taskId, string userId);
    }
}
