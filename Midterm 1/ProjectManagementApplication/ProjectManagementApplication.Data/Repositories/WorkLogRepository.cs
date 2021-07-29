using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Repositories
{
    public class WorkLogRepository : IWorkLogRepository
    {
        private readonly AppDbContext _context;

        public WorkLogRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(WorkLog entityToCreate)
        {
            await _context.WorkLogs.AddAsync(entityToCreate);

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Delete(int workLogId, int taskId, int userId)
        {
            WorkLog workLog = await _context.WorkLogs.Where(x => x.IsActive && x.TaskId == taskId && x.UserId == userId && x.Id == workLogId).FirstOrDefaultAsync();

            if (workLog != null)
            {
                workLog.IsActive = false;
            }

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Edit()
        {
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<List<WorkLog>> GetAll(int taskId, int userId)
        {
            return await _context.WorkLogs.Where(x => x.IsActive && x.TaskId == taskId && x.UserId == userId).ToListAsync();
        }

        public async Task<bool> CheckIfTaskIsAssignedToUser(int taskId, int userId)
        {
            return await _context.Tasks.AnyAsync(x => x.IsActive == true && x.Id == taskId && x.AssigneeId == userId);
        }

        public async Task<WorkLog> GetUserWorkLogByDescription(string description, int userId)
        {
            return await _context.WorkLogs.Where(x => x.Description == description && x.UserId == userId && x.IsActive).FirstOrDefaultAsync();
        }

        public async Task<WorkLog> GetUserWorkLogById(int workLogId, int taskId, int userId)
        {
            return await _context.WorkLogs.Where(x => x.IsActive && x.TaskId == taskId && x.UserId == userId && x.Id == workLogId).FirstOrDefaultAsync();
        }
    }
}
