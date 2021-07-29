using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Services
{
    public class WorkLogService : IWorkLogService
    {
        private readonly IWorkLogRepository _workLogRepository;
        private readonly ITaskRepository _taskRepository;

        public WorkLogService(IWorkLogRepository workLogRepository, ITaskRepository taskRepository)
        {
            _workLogRepository = workLogRepository;
            _taskRepository = taskRepository;
        }

        public async Task<Tuple<Messages, int>> CreateWorkLog(int taskId, string description, TimeSpan timeSpent, DateTime startDate, string userId)
        {
            if (await _workLogRepository.CheckIfTaskIsAssignedToUser(taskId, userId) == false)
            {
                return new Tuple<Messages, int>(Messages.TaskNotFound, 0);
            }

            WorkLog workLog = new WorkLog
            {
                IsActive = true,
                Description = description,
                TimeSpent = timeSpent,
                StartDate = startDate,
                TaskId = taskId,
                UserId = userId
            };

            bool isSuccess = await _workLogRepository.Create(workLog);

            if (isSuccess)
            {
                return new Tuple<Messages, int>(Messages.Success, workLog.Id);
            }

            return new Tuple<Messages, int>(Messages.OperationWasNotSuccessful, 0);
        }

        public async Task<bool> DeleteWorkLog(int workLogId, int taskId, string userId)
        {
            return await _workLogRepository.Delete(workLogId, taskId, userId);
        }

        public async Task<bool> EditWorkLog(int workLogId, string description, int taskId, DateTime startDate, TimeSpan timeSpent, string userId)
        {
            WorkLog workLogToEdit = await _workLogRepository.GetUserWorkLogById(workLogId, taskId, userId);

            if (workLogToEdit == null)
            {
                return false;
            }

            workLogToEdit.Description = description;
            workLogToEdit.StartDate = startDate;
            workLogToEdit.TimeSpent = timeSpent;

            return await _taskRepository.Edit();
        }

        public async Task<List<WorkLog>> GetAllWorkLogs(int taskId, string userId)
        {
            return await _workLogRepository.GetAll(taskId);
        }

        public async Task<WorkLog> GetUserWorkLogById(int workLogId, int taskId, string userId)
        {
            return await _workLogRepository.GetUserWorkLogById(workLogId, taskId, userId);
        }
    }
}
