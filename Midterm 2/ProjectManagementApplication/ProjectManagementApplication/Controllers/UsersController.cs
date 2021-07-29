using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.DTO.Requests.UserRequests;
using ProjectManagementApplication.DTO.Responses.UserResponses;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Api.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Create(UserCreateRequestDTO user)
        {
            switch (await _userService.CreateUser(user.Username, user.Password, user.FirstName, user.LastName, user.Role, user.TeamId))
            {
                case Messages.ChangeUsernameOrPassword:
                    return BadRequest("Change Username or Password");

                case Messages.TeamNotFound:
                    return BadRequest($"Team with Id {user.TeamId} was not found");

                case Messages.Success:
                    User userFromDb = await _userService.GetUserByName(user.Username);
                    List<string> roles = await _userService.GetUserRolesAsync(userFromDb);
                    UserCreateResponseDTO userCreateResponseDTO = new UserCreateResponseDTO();
                    MapUser(userFromDb, userCreateResponseDTO, roles.First());
                    return CreatedAtAction("Create", userCreateResponseDTO);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete]
        [Route("{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Delete(string userId)
        {
            User currentUser = await _userService.GetCurrentUser(HttpContext.User);

            switch (await _userService.DeleteUser(userId, currentUser.Id))
            {
                case Messages.YouCannotDeleteSameUserYouAreLoggedIn:
                    return BadRequest("You cannot delete the same User you are logged in");

                case Messages.UserNotFound:
                    return BadRequest($"User with Id {userId} was not found");

                case Messages.Success:
                    return StatusCode(200);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpPut]
        [Route("{userId}")]
        [Authorize(Policy = "AdminOnly")]
        public async Task<IActionResult> Edit(string userId, UserEditRequestDTO userEditRequestDTO)
        {
            bool isSuccess = await _userService.EditUser(userId, userEditRequestDTO.Username, userEditRequestDTO.Password, userEditRequestDTO.FirstName, userEditRequestDTO.LastName);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"User with id {userId} was not found! ");
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            List<UserGetAllResponseDTO> usersDTO = new List<UserGetAllResponseDTO>();

            foreach (User user in await _userService.GetAllUsers())
            {
                List<string> roles = await _userService.GetUserRolesAsync(user);

                usersDTO.Add(new UserGetAllResponseDTO
                {
                    Id = user.Id,
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = roles.Any() ? roles.First() : null
            });
        }

            return Ok(usersDTO);
    }

    private static void MapUser(User userFromDb, UserCreateResponseDTO userCreateResponseDTO, string role)
    {
        userCreateResponseDTO.Id = userFromDb.Id;
        userCreateResponseDTO.Username = userFromDb.UserName;
        userCreateResponseDTO.FirstName = userFromDb.FirstName;
        userCreateResponseDTO.LastName = userFromDb.LastName;
        userCreateResponseDTO.Role = role;
        userCreateResponseDTO.Team = userFromDb.Teams.Select(x => x.Name).FirstOrDefault();
    }
}
}


