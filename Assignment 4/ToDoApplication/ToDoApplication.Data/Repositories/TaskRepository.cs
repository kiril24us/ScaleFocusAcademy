using System.Collections.Generic;
using System.Linq;
using ToDoApplication.Data.Data;
using ToDoApplication.Data.Entities;
using ToDoApplication.Data.Interfaces;

namespace ToDoApplication.Data.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly AppDbContext _context;

        public TaskRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool Create(Task entityToCreate)
        {
            _context.Tasks.Add(entityToCreate);

            return _context.SaveChanges() != 0;
        }

        public bool Delete(Task entityToDelete)
        {
            _context.Tasks.Remove(entityToDelete);

            return _context.SaveChanges() != 0;
        }

        public bool DeleteAssignTask(UserTask sharedTaskToRemove)
        {
            _context.UsersTasks.Remove(sharedTaskToRemove);

            return _context.SaveChanges() != 0;
        }

        public bool AssignTask(UserTask userAssignedTask)
        {            
            _context.Add(userAssignedTask);

            return _context.SaveChanges() != 0;
        }

        public bool Edit()
        {
            return _context.SaveChanges() != 0;
        }

        public bool CompleteTask(int taskId)
        {
            _context.Tasks.FirstOrDefault(x => x.TaskId == taskId).IsComplete = true;

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

        public Task GetTaskById(int taskId)
        {
            return _context.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault();
        }

        public UserTask GetSharedTaskById(int userId, int taskId)
        {
            return _context.UsersTasks.Where(x => x.UserId == userId && x.TaskId == taskId).FirstOrDefault();
        }

        public List<Task> GetAll(int toDoListId)
        {
            return _context.Tasks.Where(x => x.ToDoListId == toDoListId).ToList();
        }

        public Task GetTaskByTitle(string title)
        {
            return _context.Tasks.Where(x => x.Title == title).FirstOrDefault();
        }
    }
}
