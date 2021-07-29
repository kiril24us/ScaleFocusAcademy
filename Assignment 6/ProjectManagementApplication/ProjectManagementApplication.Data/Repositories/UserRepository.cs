using Microsoft.EntityFrameworkCore;
using ProjectManagementApplication.Data.Data;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Create(User entityToCreate)
        {
            await _context.AddAsync(entityToCreate);

            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<bool> Edit()
        {
            return await _context.SaveChangesAsync() != 0;
        }

        public async Task<List<User>> GetAll()
        {
            return await _context.Users.Where(x => x.IsActive == true).ToListAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            return await _context.Users.Where(x => x.Id == userId && x.IsActive == true).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameAndPassword(string username, string password)
        {
            return await _context.Users.Where(x => x.Username == username && x.Password == password && x.IsActive == true).FirstOrDefaultAsync();
        }
    }
}
