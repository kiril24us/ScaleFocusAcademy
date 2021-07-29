using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using ToDoApplication.Data.Entities;
using ToDoApplication.DTO.DTO.Requests.ToDoListsRequests;
using ToDoApplication.DTO.DTO.Responses.TaskResponses;
using ToDoApplication.DTO.DTO.Responses.ToDoListResponses;
using ToDoApplication.Services.Auth;
using ToDoApplication.Services.Interfaces;

namespace ToDoApplication.Web.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class ToDoListsController : Controller
    {
        private readonly IToDoListService _toDoListService;
        private readonly IUserService _userService;

        public ToDoListsController(IToDoListService toDoListService, IUserService userService) : base()
        {
            _toDoListService = toDoListService;
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Create(ToDoListCreateRequestDTO toDoList)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                bool isSuccess = _toDoListService.CreateToDoList(toDoList.Title, currentUser.UserId);
                if (isSuccess)
                {
                    ToDoList toDoListFromDb = _toDoListService.GetToDoListByTitle(toDoList.Title);

                    return CreatedAtAction("Create", toDoListFromDb);
                }

                return BadRequest($"Already exists ToDo List with title {toDoList.Title}");
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("{toDoListId}")]
        public IActionResult Delete(int toDoListId)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {

                bool isSuccess = _toDoListService.DeleteToDolist(toDoListId, currentUser.UserId);
                if (isSuccess)
                {
                    return StatusCode(200);
                }

                return BadRequest($"ToDo List with id {toDoListId} was not found! ");

            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{toDoListId}")]
        public IActionResult Edit(int toDoListId, ToDoListEditRequestDTO toDoListEditRequestDTO)
        {
            User currentUser = _userService.GetCurrentUser(Request);
            if (currentUser != null)
            {
                bool isSuccess = _toDoListService.EditToDoList(toDoListId, currentUser.UserId, toDoListEditRequestDTO.Title);
                if (isSuccess)
                {
                    return StatusCode(200);
                }

                return BadRequest($"User with id {currentUser.UserId} was not found! ");
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("{toDoListId}")]
        public IActionResult Share(int toDoListId, ShareToDoListRequestDTO toDoList)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            bool isCreated = _toDoListService.CheckIfToDoListIsCreatedByUser(toDoListId, currentUser.UserId);

            if (isCreated)
            {
                if (currentUser != null)
                {
                    if (currentUser.UserId != toDoList.UserId)
                    {
                        bool isSuccess = _toDoListService.ShareToDoList(toDoListId, toDoList.UserId);

                        if (isSuccess)
                        {
                            return StatusCode(200);
                        }

                        return BadRequest($"Already shared a ToDo List with id {toDoListId} with user with id {currentUser.UserId}");
                    }
                    else
                    {
                        return BadRequest("You can not share a To Do List with you, created by you! ");
                    }
                }

                return BadRequest($"User with id {currentUser.UserId} was not found");
            }
            else
            {
                return BadRequest($"To Do List with id {toDoListId} is not created by user with id {currentUser.UserId}");
            }
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<ToDoListGetAllResponseDTO> toDoListsDTO = new List<ToDoListGetAllResponseDTO>();
            List<ToDoListGetAllResponseDTO> sharedToDoListsDTO = new List<ToDoListGetAllResponseDTO>();

            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                List<ToDoList> toDoListsFromDb = _toDoListService.GetAllListsCreatedByUser(currentUser.UserId);

                if (toDoListsFromDb.Any())
                {
                    MapToDoList(toDoListsDTO, toDoListsFromDb);
                }

                List<int> allIds = _toDoListService.GetAllIdsOfSharedToDoListsOfTheUser(currentUser.UserId);

                if (allIds.Count > 0)
                {
                    List<ToDoList> sharedToDoListsFromDb = new List<ToDoList>();

                    foreach (int id in allIds)
                    {
                        ToDoList sharedToDoList = _toDoListService.GetToDoListFromDatabaseById(id);
                        sharedToDoListsFromDb.Add(sharedToDoList);
                    }

                    MapToDoList(sharedToDoListsDTO, sharedToDoListsFromDb);
                }

                return Ok(toDoListsDTO.Concat(sharedToDoListsDTO).ToList());
            }
            else
            {
                return BadRequest("User was not found");
            }
        }

        private static void MapToDoList(List<ToDoListGetAllResponseDTO> toDoLists, List<ToDoList> toDoListsFromDb)
        {
            foreach (ToDoList toDoList in toDoListsFromDb)
            {
                ToDoListGetAllResponseDTO toDoListDTO = new ToDoListGetAllResponseDTO();

                toDoListDTO.ToDoListId = toDoList.ToDoListId;
                toDoListDTO.Title = toDoList.Title;
                toDoListDTO.CreatedById = toDoList.CreatedById;
                toDoListDTO.CreatedOn = toDoList.CreatedOn;
                toDoListDTO.LastModifiedById = toDoList.LastModifiedById;
                toDoListDTO.LastModifiedOn = toDoList.LastModifiedOn;

                toDoLists.Add(toDoListDTO);

                toDoListDTO.Tasks.Add(new TaskGetAllForToDoListResponseDTO()
                {
                    CountTasks = toDoList.Tasks.Count
                });
            }
        }
    }
}
