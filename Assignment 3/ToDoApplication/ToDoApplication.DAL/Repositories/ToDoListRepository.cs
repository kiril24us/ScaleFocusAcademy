using System.Collections.Generic;
using System.Linq;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Repositories
{
    public class ToDoListRepository : IToDoListRepository
    {
        private readonly AppDbContext _context;

        public ToDoListRepository(AppDbContext context)
        {
            _context = context;
        }
        public bool Create(ToDoList entityToCreate)
        {
            _context.ToDoLists.Add(entityToCreate);
            object result = _context.SaveChanges();
            return result != null;
        }

        /// <summary>
        /// Delete To Do List Created By User
        /// </summary>
        /// <param name="entityToDelete"></param>
        /// <returns>True if deleted or false</returns>
        public bool Delete(ToDoList entityToDelete)
        {
            _context.ToDoLists.Remove(entityToDelete);
            object result = _context.SaveChanges();
            return result != null;
        }

        /// <summary>
        /// Delete Shared List with User
        /// </summary>
        /// <param name="sharedList"></param>
        /// <returns>True if deleted or false</returns>
        public bool DeleteSharedToDoList(UserToDoList sharedList)
        {
            _context.UsersToDoLists.Remove(sharedList);
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool Edit()
        {
            object result = _context.SaveChanges();
            return result != null;
        }

        /// <summary>
        /// Share To Do List with User
        /// </summary>
        /// <param name="toDoListId"></param>
        /// <param name="userIdToBeShared"></param>
        /// <returns></returns>
        public bool ShareToDoList(UserToDoList userToDoList)
        {
            _context.UsersToDoLists.Add(userToDoList);
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool CheckIfToDoListExistInTheDatabase(int toDoListId)
        {
            return _context.ToDoLists.Any(x => x.ToDoListId == toDoListId);
        }

        public bool CheckIfToDoListExistByTitle(string title)
        {
            return _context.ToDoLists.Any(x => x.Title == title);
        }

        public bool CheckIfToDoListIsCreatedByUser(int toDoListId, int userId)
        {
            return _context.ToDoLists.Any(x => x.ToDoListId == toDoListId && x.CreatedById == userId);
        }

        public bool CheckIfToDoListExistInSharedToDoListsOfUser(int toDoListId, int userId)
        {
            return _context.UsersToDoLists.Any(x => x.ToDoListId == toDoListId && x.UserId == userId);
        }

        public bool CheckIfToDoListIsSharedByOtherUsers(int toDoListId)
        {
            return _context.UsersToDoLists.Any(x => x.ToDoListId == toDoListId);
        }

        public bool CheckIfToDoListIsAlreadySharedWithThatUser(int toDoListId, int userIdToBeShared)
        {
            return _context.UsersToDoLists.Any(x => x.ToDoListId == toDoListId && x.UserId == userIdToBeShared);
        }

        public bool CheckIfThereAreCreatedTasksToToDoList(int toDoListId)
        {
            return _context.Tasks.Any(x => x.ToDoListId == toDoListId);
        }
       
        public List<int> GetAllIdsOfSharedToDoListsOfTheUser(int userId)
        {
            return _context.UsersToDoLists.Where(x => x.UserId == userId).Select(x => x.ToDoListId).ToList();
        }

        public List<ToDoList> GetAllListsCreatedByUser(int userId)
        {
            return _context.ToDoLists.Where(x => x.CreatedById == userId).ToList();
        }

        public List<int> GetUsersIdsWithSharedList(int toDoListId)
        {
            return _context.UsersToDoLists.Where(x => x.ToDoListId == toDoListId).Select(x => x.UserId).ToList();
        }

        public UserToDoList GetSharedList(int toDoListId, int userId)
        {
            return _context.UsersToDoLists.FirstOrDefault(x => x.ToDoListId == toDoListId && x.UserId == userId);
        }

        public ToDoList GetToDoListById(int toDoListId)
        {
            return _context.ToDoLists.FirstOrDefault(x => x.ToDoListId == toDoListId);
        }

        public int GetToDoListIdWhichContainsTask(int taskId)
        {
            return _context.Tasks.Where(x => x.TaskId == taskId).Select(x => x.ToDoListId).FirstOrDefault();
        }       
    }
}
