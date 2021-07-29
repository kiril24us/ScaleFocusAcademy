using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly AppDbContext _context;

        public ProjectRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Project entityToCreate)
        {
            await _context.Projects.AddAsync(entityToCreate);

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Delete(int projectId, string userId)
        {
            Project project = await _context.Projects.Where(x => x.Id == projectId && x.UserId == userId && x.IsActive == true).FirstOrDefaultAsync();

            if(project != null)
            {
                project.IsActive = false;
            }
            
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Edit()
        {
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<List<Project>> GetAll()
        {
            return await _context.Projects.Where(x => x.IsActive).ToListAsync();
        }

        public async Task<List<Project>> GetMineProjects(string userId)
        {
            return await _context.Projects.Where(x => x.IsActive && (x.UserId == userId || (x.Teams.Any(x => x.IsActive == true) && x.Teams.Any(x => x.Members.Any(x => x.Id == userId && x.IsActive == true))))).ToListAsync();
        }

        public async Task<Project> GetProjectById(int projectId, string userId)
        {
            return await _context.Projects.Where(x => x.Id == projectId && x.IsActive == true && x.UserId == userId).FirstOrDefaultAsync();
        }

        public async Task<Project> GetProjectByName(string name)
        {
            return await _context.Projects.Where(x => x.Name == name && x.IsActive == true).FirstOrDefaultAsync();
        }
    }
}
