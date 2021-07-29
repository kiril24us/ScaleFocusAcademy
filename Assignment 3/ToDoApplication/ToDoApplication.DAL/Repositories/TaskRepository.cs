using System.Collections.Generic;
using System.Linq;
using ToDoApplication.DAL.Data;
using ToDoApplication.DAL.Entities;

namespace ToDoApplication.DAL.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Create(Entities.Task entityToCreate)
        {
            _context.Tasks.Add(entityToCreate);
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool Delete(Entities.Task entityToDelete)
        {
            _context.Tasks.Remove(entityToDelete);
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool DeleteAssignTask(UserTask sharedTaskToRemove)
        {
            _context.UsersTasks.Remove(sharedTaskToRemove);
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool AssignTask(UserTask userTask)
        {            
            _context.Add(userTask);
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool Edit()
        {
            object result = _context.SaveChanges();
            return result != null;
        }

        public bool CompleteTask(int taskId)
        {
            _context.Tasks.First(x => x.TaskId == taskId).IsComplete = true;
            return _context.SaveChanges() != 0;
        }

        public bool CheckIfThereIsAlreadyTaskWithGivenTitle(string title)
        {
            return _context.Tasks.Any(x => x.Title == title);
        }

        public bool CheckIfTaskExistInDatabaseById(int taskId)
        {
            return _context.Tasks.Any(x => x.TaskId == taskId);
        }

        public bool CheckIfUserIsCreatorOfTheTask(int taskId, int userId)
        {
            return _context.Tasks.Any(x => x.TaskId == taskId && x.CreatedById == userId);
        }

        public bool CheckIfTaskIsAlreadyAssignToUser(int userId, int taskId)
        {
            return _context.UsersTasks.Any(x => x.UserId == userId && x.TaskId == taskId);
        }

        public bool CheckIfThereIsAssignedTaskToTheUser(int userId, int taskId)
        {
            return _context.UsersTasks.Any(x => x.UserId == userId && x.TaskId == taskId);
        }

        public List<int> GetAllTaskIdsCreatedToToDoList(int toDoListId)
        {
            return _context.Tasks.Where(x => x.ToDoListId == toDoListId).Select(x => x.TaskId).ToList();
        }

        public Entities.Task GetTaskById(int taskId)
        {
            return _context.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault();
        }

        public UserTask GetSharedTaskById(int userId, int taskId)
        {
            return _context.UsersTasks.Where(x => x.UserId == userId && x.TaskId == taskId).FirstOrDefault();
        }

        public List<Entities.Task> GetAllTasks(int toDoListId)
        {
            return _context.Tasks.Where(x => x.ToDoListId == toDoListId).ToList();
        }
    }
}
