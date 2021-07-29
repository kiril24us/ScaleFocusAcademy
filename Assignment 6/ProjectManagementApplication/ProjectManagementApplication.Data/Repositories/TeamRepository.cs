using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Repositories
{
    public class TeamRepository : ITeamRepository
    {
        private readonly AppDbContext _context;

        public TeamRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(Team entityToCreate)
        {
            await _context.Teams.AddAsync(entityToCreate);

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Edit()
        {
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<List<Team>> GetAll()
        {
            return await _context.Teams.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<Team> GetTeamById(int teamId)
        {
            return await _context.Teams.Where(x => x.Id == teamId && x.IsActive == true).FirstOrDefaultAsync();
        }

        public async Task<Team> GetTeamByName(string name)
        {
            return await _context.Teams.Where(x => x.Name == name && x.IsActive == true).FirstOrDefaultAsync();
        }
    }
}
