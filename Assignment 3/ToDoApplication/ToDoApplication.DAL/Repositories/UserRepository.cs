using System.Collections.Generic;
using System.Linq;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Repositories
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
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool Delete(User entityToDelete)
        {
            _context.Remove(entityToDelete);
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool Edit()
        {
            object result = _context.SaveChanges();
            return result != null;
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

        public List<User> GetAllUsers()
        {
            return _context.Users.ToList();
        }       
    }
}
