using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ToDoApplication.Data.Entities;
using ToDoApplication.DTO.DTO.Requests.UserRequests;
using ToDoApplication.DTO.DTO.Responses.UserResponses;
using ToDoApplication.Services.Auth;
using ToDoApplication.Services.Interfaces;

namespace ToDoApplication.Web.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService) : base()
        {
            _userService = userService;
        }

        [HttpPost]
        public IActionResult Create(UserCreateRequestDTO user)
        {
            User currentUser = _userService.GetCurrentUser(Request);
            if (currentUser != null)
            {
                if (currentUser.Role.ToString() == "Admin")
                {
                    bool isSuccess = _userService.CreateUser(user.Username, user.Password, user.FirstName, user.LastName, user.Role);
                    if (isSuccess)
                    {
                        User userFromDb = _userService.GetUsernameByUsernameAndPassword(user.Username, user.Password);

                        return CreatedAtAction("Create", userFromDb);
                    }

                    return BadRequest("Change username or password");
                }
                else
                {
                    return Unauthorized("Only Users with administrative privileges can create Users! ");
                }
            }

            return BadRequest();
        }

        [HttpDelete]
        [Route("{userId}")]
        public IActionResult Delete(int userId)
        {
            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                if (currentUser.Role.ToString() == "Admin")
                {
                    bool isSuccess = _userService.DeleteUser(userId);
                    if (isSuccess)
                    {
                        return StatusCode(200);
                    }

                    return BadRequest($"User with id {userId} was not found! ");
                }
                else
                {
                    return Unauthorized("Only Users with administrative privileges can edit Users! ");
                }
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("{userId}")]
        public IActionResult Edit(int userId, UserEditRequestDTO userEditRequestDTO)
        {
            User currentUser = _userService.GetCurrentUser(Request);
            if (currentUser != null)
            {
                if (currentUser.Role.ToString() == "Admin")
                {
                    bool isSuccess = _userService.EditUser(userId, userEditRequestDTO.Username, userEditRequestDTO.Password, userEditRequestDTO.FirstName, userEditRequestDTO.LastName);
                    if (isSuccess)
                    {
                        return StatusCode(200);
                    }

                    return BadRequest($"User with id {userId} was not found! ");
                }
                else
                {
                    return Unauthorized("Only Users with administrative privileges can delete Users! ");
                }
            }

            return BadRequest();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<UserGetAllResponseDTO> usersDTO = new List<UserGetAllResponseDTO>();

            User currentUser = _userService.GetCurrentUser(Request);

            if (currentUser != null)
            {
                foreach (User _user in _userService.GetAllUsers(currentUser.UserId))
                {
                    usersDTO.Add(new UserGetAllResponseDTO
                    {
                        UserId = _user.UserId,
                        Username = _user.Username,
                        FirstName = _user.FirstName,
                        LastName = _user.LastName,
                        Role = _user.Role,
                        CreatedById = _user.CreatedById,
                        CreatedOn = _user.CreatedOn,
                        LastModifiedById = _user.LastModifiedById,
                        LastModifiedOn = _user.LastModifiedOn
                    });
                }
            }
            else
            {
                return BadRequest($"User was not found");
            }

            return Ok(usersDTO);
        }
    }
}
