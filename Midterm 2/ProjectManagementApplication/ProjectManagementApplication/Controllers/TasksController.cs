using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.DTO.Requests.TaskRequests;
using ProjectManagementApplication.DTO.Responses.TaskReponses;
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
        private readonly IUserService _userService;

        public TasksController(ITaskService taskService, IUserService userService)
        {
            _taskService = taskService;
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Policy = "CreatorOrTeamMemberOnly")]
        public async Task<IActionResult> Create(TaskCreateRequestDTO task)
        {
            User currentUser = await _userService.GetCurrentUser(HttpContext.User);

            Tuple<Messages, int> tuple = await _taskService.CreateTask(task.Name, task.Status, task.ProjectId, currentUser.Id);

            switch (tuple.Item1)
            {
                case Messages.AlreadyExistTask:
                    return BadRequest($"Already Exist Task with name {task.Name}");

                case Messages.ProjectNotFound:
                    return BadRequest($"Project was not found");

                case Messages.OperationWasNotSuccessful:
                    return BadRequest($"Operation was no successful");

                case Messages.Success:
                    Task taskFromDb = await _taskService.GetTaskById(tuple.Item2, currentUser.Id);
                    TaskCreateResponseDTO taskCreateResponseDTO = new TaskCreateResponseDTO();
                    MapTask(taskFromDb, taskCreateResponseDTO);
                    return CreatedAtAction("Create", taskCreateResponseDTO);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete]
        [Authorize(Policy = "CreatorOrTeamMemberOnly")]
        [Route("{taskId}")]
        public async Task<IActionResult> Delete(int taskId, TaskDeleteRequestDTO taskDeleteRequestDTO)
        {
            User currentUser = await _userService.GetCurrentUser(HttpContext.User);

            bool isSuccess = await _taskService.DeleteTask(taskId, taskDeleteRequestDTO.ProjectId, currentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Project with Id {taskDeleteRequestDTO.ProjectId} is created by you or is assigned to a Team that you are a member of");
        }

        [HttpPut]
        [Authorize(Policy = "CreatorOrTeamMemberOnly")]
        [Route("{taskId}")]
        public async Task<IActionResult> Edit(int taskId, TaskEditRequestDTO taskEditRequestDTO)
        {
            User currentUser = await _userService.GetCurrentUser(HttpContext.User);

            bool isSuccess = await _taskService.EditTask(taskId, taskEditRequestDTO.Name, taskEditRequestDTO.Status, taskEditRequestDTO.AssigneeId, currentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Project with Id {taskEditRequestDTO.ProjectId} is created by you or is assigned to a Team that you are a member of");
        }

        [HttpPost]
        [Authorize(Policy = "CreatorOrTeamMemberOnly")]
        public async Task<IActionResult> AssignTaskToAUser(int taskId, AssignTaskToAUserRequestDTO assignUserToATeamRequestDTO)
        {   
            User currentUser = await _userService.GetCurrentUser(HttpContext.User);

            bool isSuccess = await _taskService.AssignTask(taskId, assignUserToATeamRequestDTO.AssigneeId, currentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Be sure that Project is created by you or is assigned to a Team that you are a member of and be sure that you are creator of the Task with id {taskId}");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> GetTask(int taskId)
        {
            User currentUser = await _userService.GetCurrentUser(HttpContext.User);

            Task taskFromDb = await _taskService.GetTaskById(taskId, currentUser.Id);

            if(taskFromDb == null)
            {
                return BadRequest($"Be sure that Project related to Task is created by you or is assigned to a Team that you are a member of and be sure that you are creator of the Task with id {taskId}");
            }

            TaskCreateResponseDTO taskCreateResponseDTO = new TaskCreateResponseDTO();
            MapTask(taskFromDb, taskCreateResponseDTO);

            return Ok(taskCreateResponseDTO);
        }

        [HttpGet]
        [Authorize(Policy = "CreatorOrTeamMemberOnly")]
        public async Task<IActionResult> GetAll([FromQuery]TaskDeleteRequestDTO taskDeleteRequestDTO)
        {
            User currentUser = await _userService.GetCurrentUser(HttpContext.User);

            List<TaskCreateResponseDTO> tasksDTO = new List<TaskCreateResponseDTO>();

            foreach (Task task in await _taskService.GetAllTasks(taskDeleteRequestDTO.ProjectId, currentUser.Id))
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

        private static void MapTask(Task taskFromDb, TaskCreateResponseDTO taskCreateResponseDTO)
        {
            taskCreateResponseDTO.Id = taskFromDb.Id;
            taskCreateResponseDTO.Name = taskFromDb.Name;
            taskCreateResponseDTO.Status = taskFromDb.Status.ToString();
            taskCreateResponseDTO.ProjectId = taskFromDb.ProjectId;
            taskCreateResponseDTO.UserId = taskFromDb.CreatorId;
        }
    }
}

