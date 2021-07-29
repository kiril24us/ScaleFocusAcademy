using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Task = ProjectManagementApplication.Data.Entities.Task;

namespace ProjectManagementApplication.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Task entityToCreate)
        {
            await _context.Tasks.AddAsync(entityToCreate);

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Delete(int taskId, int projectId, int userId)
        {
            Task task = await _context.Tasks.Where(x => x.Id == taskId && x.IsActive &&
                   (x.Project.UserId == userId && x.Project.IsActive || (x.Project.Teams.Any(x => x.IsActive == true)
                   && x.Project.Teams.Any(x => x.Members.Any(x => x.Id == userId) && x.IsActive == true))))
                   .FirstOrDefaultAsync();

            if (task != null)
            {
                task.IsActive = false;
            }

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Edit()
        {
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<List<Task>> GetAllTasks(int projectId, int userId)
        {
            return await _context.Tasks.Where(x => x.ProjectId == projectId && x.IsActive && (x.Project.UserId == userId && x.Project.IsActive || (x.Project.Teams.Any(x => x.IsActive)
                 && x.Project.Teams.Any(x => x.Members.Any(x => x.Id == userId) && x.IsActive)))).ToListAsync();
        }

        public async Task<Task> GetTaskById(int taskId, int userId)
        {
            return await _context.Tasks.Where(x => x.IsActive && x.Id == taskId && x.CreatorId == userId && (x.Project.UserId == userId && x.Project.IsActive || (x.Project.Teams.Any(x => x.IsActive)
                 && x.Project.Teams.Any(x => x.Members.Any(x => x.Id == userId) && x.IsActive)))).FirstOrDefaultAsync();
        }

        public async Task<Task> GetTaskByName(string name)
        {
            return await _context.Tasks.Where(x => x.IsActive == true && x.Name == name).FirstOrDefaultAsync();
        }
    }
}
