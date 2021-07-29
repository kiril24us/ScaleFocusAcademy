using System.Collections.Generic;
using System.Linq;
using ToDoApplication.Data.Data;
using ToDoApplication.Data.Entities;
using ToDoApplication.Data.Interfaces;

namespace ToDoApplication.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context)
        {
            _context = context;
        }
        
        public bool Create(User entityToCreate)
        {
            _context.Add(entityToCreate);

            return _context.SaveChanges() != 0;
        }

        public bool Delete(User entityToDelete)
        {
            _context.Remove(entityToDelete);

            return _context.SaveChanges() != 0;
        }

        public bool Edit()
        {
            return _context.SaveChanges() != 0;
        }

        public bool CheckIfUserExistById(int userId)
        {
            return _context.Users.Any(x => x.UserId == userId);
        }

        public bool CheckIfUserIsCreatorOfToDoLists(int userId)
        {
            return _context.ToDoLists.Any(x => x.CreatedById == userId);
        }

        public User GetUserById(int userId)
        {
            return _context.Users.FirstOrDefault(x => x.UserId == userId);
        }

        public User GetUsernameByUsernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(x => x.Username == username && x.Password == password);
        }

        public List<User> GetAll(int userId)
        {
            return _context.Users.ToList();
        }
    }
}
