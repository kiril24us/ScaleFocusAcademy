using ProjectManagementApplication.Data.Enums;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = ProjectManagementApplication.Data.Entities.Task;

namespace ProjectManagementApplication.Services.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<Tuple<Messages, int>> CreateTask(string name, int status, int projectId, string userId)
        {
            if (await _taskRepository.GetTaskByName(name) != null)
            {
                return new Tuple<Messages, int>(Messages.AlreadyExistTask, 0);
            }

            Status statusAsEnum = Enum.Parse<Status>(status.ToString());

            try
            {
                Task task = new Task
                {
                    Name = name,
                    Status = statusAsEnum,
                    CreatorId = userId,
                    IsActive = true,
                    ProjectId = projectId
                };

                bool isSuccess = await _taskRepository.Create(task);

                if (isSuccess)
                {
                    return new Tuple<Messages, int>(Messages.Success, task.Id);
                }

                return new Tuple<Messages, int>(Messages.OperationWasNotSuccessful, 0);
            }
            catch (Exception)
            {
                return new Tuple<Messages, int>(Messages.ProjectNotFound, 0);
            }
        }

        public async Task<bool> DeleteTask(int taskId, int projectId, string userId)
        {
            return await _taskRepository.Delete(taskId, projectId, userId);
        }

        public async Task<bool> EditTask(int taskId, string name, int status, string assigneeId, string userId)
        {
            Task taskToEdit = await _taskRepository.GetTaskById(taskId, userId);

            if (taskToEdit == null)
            {
                return false;
            }

            Status statusAsEnum = Enum.Parse<Status>(status.ToString());

            taskToEdit.Name = name;
            taskToEdit.Status = statusAsEnum;
            taskToEdit.AssigneeId = assigneeId;

            return await _taskRepository.Edit();
        }

        public async Task<bool> AssignTask(int taskId, string assigneeId, string userId)
        {
            Task taskToAssign = await _taskRepository.GetTaskById(taskId, userId);
            
            if (taskToAssign == null)
            {
                return false;
            }

            taskToAssign.AssigneeId = assigneeId;

            return await _taskRepository.Edit();
        }

        public Task<List<Task>> GetAllTasks(int projectId, string userId)
        {
            return _taskRepository.GetAllTasks(projectId, userId);
        }

        public async Task<Task> GetTaskById(int taskId, string userId)
        {
            return await _taskRepository.GetTaskById(taskId, userId);
        }
    }
}
