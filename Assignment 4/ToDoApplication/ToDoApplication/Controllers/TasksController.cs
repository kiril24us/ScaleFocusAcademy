using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using ToDoApplication.Data.Entities;
using ToDoApplication.DTO.DTO.Requests.TasksRequests;
using ToDoApplication.DTO.DTO.Responses.TaskResponses;
using ToDoApplication.Services.Auth;
using ToDoApplication.Services.Interfaces;

namespace ToDoApplication.Web.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class TasksController : Controller
    {
        private readonly ITaskService _taskService;
        private readonly IUserService _userService;
        private readonly IToDoListService _toDoListService;

        public TasksController(ITaskService taskService, IUserService userService, IToDoListService toDoListService) : base()
        {
            _taskService = taskService;
            _userService = userService;
            _toDoListService = toDoListService;
        }

        [HttpPost]
        public IActionResult Create(TaskCreateRequestDTO task)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                bool isSuccess = _taskService.CreateTaskInToDoList(task.ToDoListId, task.Title, task.Description, task.IsComplete, currentUser.UserId);
                if (isSuccess)
                {
                    Task taskFromDb = _taskService.GetTaskByTitle(task.Title);

                    return CreatedAtAction("Create", taskFromDb);
                }

                return BadRequest($"Already exists Task with title {task.Title}");
            }

            return BadRequest($"User with id {currentUser.UserId} was not found");
        }

        [HttpDelete]
        [Route("{taskId}")]
        public IActionResult Delete(int taskId, TaskDeleteRequestDTO taskDeleteRequestDTO)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(taskDeleteRequestDTO.ToDoListId, currentUser.UserId);
                bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(taskDeleteRequestDTO.ToDoListId, currentUser.UserId);

                if (isExist || isExistInSharedToDoListsCollection)
                {
                    bool isExistTask = _taskService.CheckIfTaskExistInDatabaseById(taskId);

                    if (isExistTask)
                    {
                        bool isCreator = _taskService.CheckIfUserIsCreatorOfTheTask(taskId, currentUser.UserId);

                        if (isCreator)
                        {
                            bool isSuccess = _taskService.DeleteTask(taskDeleteRequestDTO.ToDoListId, taskId);

                            if (isSuccess)
                            {
                                return StatusCode(200);
                            }

                            return BadRequest("Operation was not successful");
                        }
                        else
                        {
                            bool isSuccess = _taskService.DeleteAssignTask(currentUser.UserId, taskId);

                            if (isSuccess)
                            {
                                return StatusCode(200);
                            }

                            return BadRequest("Operation was not successful");
                        }
                    }
                    else
                    {
                        return BadRequest($"Task with id {taskId} was not found!");

                    }
                }
                else
                {
                    return BadRequest("To Do List was not found");
                }
            }

            return BadRequest($"User with id {currentUser.UserId} was not found");
        }

        [HttpPut]
        [Route("{taskId}")]
        public IActionResult Edit(int taskId, TaskEditRequestDTO taskEditRequestDTO)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(taskEditRequestDTO.ToDoListId, currentUser.UserId);
                bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(taskEditRequestDTO.ToDoListId, currentUser.UserId);

                if (isExist || isExistInSharedToDoListsCollection)
                {
                    if (_taskService.CheckIfTaskExistInDatabaseById(taskId))
                    {
                        bool isSuccess = _taskService.EditTask(taskId, taskEditRequestDTO.Title, taskEditRequestDTO.Description, taskEditRequestDTO.IsComplete, currentUser.UserId);
                        if (isSuccess)
                        {
                            return StatusCode(200);
                        }
                    }
                    else
                    {
                        return BadRequest($"Task with id {taskId} was not found!");
                    }
                }
                else
                {
                    return BadRequest("To Do List was not found");
                }

            }

            return BadRequest();
        }

        [HttpPost]
        [Route("{taskId}")]
        public IActionResult Assign(int taskId, TaskAssignRequestDTO taskAssignRequestDTO)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                if (_userService.CheckIfUserExistById(taskAssignRequestDTO.UserId))
                {
                    bool isExist = _taskService.CheckIfTaskExistInDatabaseById(taskId);

                    if (isExist)
                    {
                        int toDoListId = _toDoListService.GetToDoListIdWhichContainsTask(taskId);

                        if (toDoListId > 0)
                        {
                            bool isExistToDoListInUser = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, currentUser.UserId);
                            bool isExistSharedToDoListInUser = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, currentUser.UserId);

                            if (isExistToDoListInUser || isExistSharedToDoListInUser)
                            {
                                Task task = _taskService.GetTaskById(taskId);

                                if (task.CreatedById == taskAssignRequestDTO.UserId)
                                {
                                    return BadRequest("You cannot assign a task to yourself! ");
                                }
                                else
                                {
                                    bool isSuccess = _taskService.AssignTask(taskAssignRequestDTO.UserId, taskId);

                                    if (isSuccess)
                                    {
                                        return StatusCode(200);
                                    }

                                    return BadRequest($"You already assign a Task with Id {taskId} to User with Id {taskAssignRequestDTO.UserId} ");
                                }
                            }
                            else
                            {
                                return BadRequest($"User with id {currentUser.UserId} doesn't have ToDo List with id {toDoListId} ");
                            }
                        }
                    }
                    else
                    {
                        return BadRequest($"Task with id {taskId} was not found ");
                    }
                }
                else
                {
                    return BadRequest($"User with id {taskAssignRequestDTO.UserId} was not found ");
                }
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("{taskId}")]
        public IActionResult Complete(int taskId)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                bool isExist = _taskService.CheckIfTaskExistInDatabaseById(taskId);

                if (isExist)
                {
                    int toDoListId = _toDoListService.GetToDoListIdWhichContainsTask(taskId);

                    if (toDoListId > 0)
                    {
                        bool isExistToDoListInUser = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, currentUser.UserId);
                        bool isExistSharedToDoListInUser = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, currentUser.UserId);

                        if (isExistToDoListInUser || isExistSharedToDoListInUser)
                        {
                            bool isSuccess = _taskService.CompleteTask(taskId);

                            if (isSuccess)
                            {
                                return StatusCode(200);
                            }

                            return BadRequest($"The task with Id {taskId} is already completed ");
                        }
                    }
                }
                else
                {
                    return BadRequest($"No Task with id {taskId} was found");
                }

            }

            return BadRequest();
        }

        [HttpGet]
        [Route("{toDoListId}")]
        public IActionResult GetAll(int toDoListId)
        {
            List<TaskGetAllResponseDTO> tasksDTO = new List<TaskGetAllResponseDTO>();

            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                bool isExistToDoList = _toDoListService.CheckIfToDoListExistInTheDatabase(toDoListId);

                if (isExistToDoList)
                {
                    bool isExist = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, currentUser.UserId);
                    bool isExistInSharedToDoListsCollection = _toDoListService.CheckIfToDoListExistInSharedToDoListsOfUser(toDoListId, currentUser.UserId);

                    if (isExist || isExistInSharedToDoListsCollection)
                    {
                        List<Task> tasksFromDb = _taskService.GetAllTasks(toDoListId);
                        if(tasksFromDb.Any())
                        {
                            MapTask(tasksDTO, tasksFromDb);
                        }
                    }
                    else
                    {
                        return BadRequest($"ToDo List with Id {toDoListId} exists in the database but is not shared with you or created by you");
                    }
                }
                else
                {
                    return BadRequest("ToDo List was not found");
                }
            }
            else
            {
                return BadRequest("User was not found");
            }

            return Ok(tasksDTO);
        }

        private void MapTask(List<TaskGetAllResponseDTO> tasksDTO, List<Task> tasksFromDb)
        {
            foreach (Task task in tasksFromDb)
            {
                TaskGetAllResponseDTO taskDTO = new TaskGetAllResponseDTO();

                taskDTO.TaskId = task.TaskId;
                taskDTO.ToDoListId = task.ToDoListId;
                taskDTO.Title = task.Title;
                taskDTO.Description = task.Description;
                taskDTO.IsComplete = task.IsComplete;
                taskDTO.CreatedById = task.CreatedById;
                taskDTO.CreatedOn = task.CreatedOn;
                taskDTO.LastModifiedById = task.LastModifiedById;
                taskDTO.LastModifiedOn = task.LastModifiedOn;

                tasksDTO.Add(taskDTO);
            }
        }
    }
}
