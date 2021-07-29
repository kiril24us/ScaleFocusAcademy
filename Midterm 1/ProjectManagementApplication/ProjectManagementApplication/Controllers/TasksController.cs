using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Api.Attributes;
using ProjectManagementApplication.DTO.Requests.TaskRequests;
using ProjectManagementApplication.DTO.Responses.TaskReponses;
using ProjectManagementApplication.Services.Auth;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task = ProjectManagementApplication.Data.Entities.Task;

namespace ProjectManagementApplication.Api.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IAuthentication _authentication;

        public TasksController(ITaskService taskService, IAuthentication authentication)
        {
            _taskService = taskService;
            _authentication = authentication;
        }

        [HttpPost]
        [Login]
        public async Task<IActionResult> Create(TaskCreateRequestDTO task)
        {
            Tuple<Messages, int> tuple = await _taskService.CreateTask(task.Name, task.Status, task.ProjectId, Authentication.CurrentUser.Id);

            switch (tuple.Item1)
            {
                case Messages.AlreadyExistTask:
                    return BadRequest($"Already Exist Task with name {task.Name}");

                case Messages.ProjectNotFound:
                    return BadRequest($"Project was not found");

                case Messages.OperationWasNotSuccessful:
                    return BadRequest($"Operation was no successful");

                case Messages.Success:
                    Task taskFromDb = await _taskService.GetTaskById(tuple.Item2, Authentication.CurrentUser.Id);
                    TaskCreateResponseDTO taskCreateResponseDTO = new TaskCreateResponseDTO();
                    MapTask(taskFromDb, taskCreateResponseDTO);
                    return CreatedAtAction("Create", taskCreateResponseDTO);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete]
        [Login]
        [Route("{taskId}")]
        public async Task<IActionResult> Delete(int taskId, TaskDeleteRequestDTO taskDeleteRequestDTO)
        {
            bool isSuccess = await _taskService.DeleteTask(taskId, taskDeleteRequestDTO.ProjectId, Authentication.CurrentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Project with Id {taskDeleteRequestDTO.ProjectId} is created by you or is assigned to a Team that you are a member of");
        }

        [HttpPut]
        [Login]
        [Permissions]
        [Route("{taskId}")]
        public async Task<IActionResult> Edit(int taskId, TaskEditRequestDTO taskEditRequestDTO)
        {
            bool isSuccess = await _taskService.EditTask(taskId, taskEditRequestDTO.Name, taskEditRequestDTO.Status, taskEditRequestDTO.AssigneeId, Authentication.CurrentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Project with Id {taskEditRequestDTO.ProjectId} is created by you or is assigned to a Team that you are a member of");
        }

        [HttpPost]
        [Login]
        public async Task<IActionResult> AssignTaskToAUser(int taskId, AssignTaskToAUserRequestDTO assignUserToATeamRequestDTO)
        {
            bool isSuccess = await _taskService.AssignTask(taskId, assignUserToATeamRequestDTO.AssigneeId, Authentication.CurrentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Project is created by you or is assigned to a Team that you are a member of and be sure that you are creator of the Task with id {taskId}");
        }

        [HttpPost]
        [Login]
        public async Task<IActionResult> GetTask(int taskId)
        {
            Task taskFromDb = await _taskService.GetTaskById(taskId, Authentication.CurrentUser.Id);

            if(taskFromDb == null)
            {
                return BadRequest($"Be sure that Project related to Task is created by you or is assigned to a Team that you are a member of and be sure that you are creator of the Task with id {taskId}");
            }

            TaskCreateResponseDTO taskCreateResponseDTO = new TaskCreateResponseDTO();
            MapTask(taskFromDb, taskCreateResponseDTO);

            return Ok(taskCreateResponseDTO);
        }

        [HttpGet]
        [Login]
        public async Task<IActionResult> GetAll([FromQuery]TaskDeleteRequestDTO taskDeleteRequestDTO)
        {
            List<TaskCreateResponseDTO> tasksDTO = new List<TaskCreateResponseDTO>();

            foreach (Task task in await _taskService.GetAllTasks(taskDeleteRequestDTO.ProjectId, Authentication.CurrentUser.Id))
            {
                tasksDTO.Add(new TaskCreateResponseDTO
                {
                    Id = task.Id,
                    Name = task.Name,
                    ProjectId = task.ProjectId,
                    UserId = task.CreatorId,
                    Status = task.Status.ToString()
                });
            }

            return Ok(tasksDTO);
        }

        private void MapTask(Task taskFromDb, TaskCreateResponseDTO taskCreateResponseDTO)
        {
            taskCreateResponseDTO.Id = taskFromDb.Id;
            taskCreateResponseDTO.Name = taskFromDb.Name;
            taskCreateResponseDTO.Status = taskFromDb.Status.ToString();
            taskCreateResponseDTO.ProjectId = taskFromDb.ProjectId;
            taskCreateResponseDTO.UserId = taskFromDb.CreatorId;
        }
    }
}

