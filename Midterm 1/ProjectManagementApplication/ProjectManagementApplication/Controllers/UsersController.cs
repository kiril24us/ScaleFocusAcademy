using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Api.Attributes;
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
        private readonly IAuthentication _authentication;

        public UsersController(IUserService userService, IAuthentication authentication)
        {
            _userService = userService;
            _authentication = authentication;
        }

        [HttpGet]
        public async Task<IActionResult> Login([FromQuery] UserLoginRequestDTO userLoginRequestDTO)
        {
            User currentUser = await _authentication.Login(userLoginRequestDTO.Username, userLoginRequestDTO.Password);

            if (currentUser != null)
            {
                UserLoginResponseDTO userLoginResponseDTO = new UserLoginResponseDTO();
                userLoginResponseDTO.Id = currentUser.Id;

                return Ok(userLoginResponseDTO);
            }

            return BadRequest("No User found, try with different username or password");
        }

        [HttpPost]
        [Login]
        [Permissions]
        public async Task<IActionResult> Create(UserCreateRequestDTO user)
        {
            switch (await _userService.CreateUser(user.Username, user.Password, user.FirstName, user.LastName, user.Role, user.TeamId))
            {
                case Messages.ChangeUsernameOrPassword:
                    return BadRequest("Change Username or Password");

                case Messages.TeamNotFound:
                    return BadRequest($"Team with Id {user.TeamId} was not found");

                case Messages.Success:
                    User userFromDb = await _userService.GetUserByUsernameAndPassword(user.Username, user.Password);
                    UserCreateResponseDTO userCreateResponseDTO = new UserCreateResponseDTO();
                    MapUser(userFromDb, userCreateResponseDTO);
                    return CreatedAtAction("Create", userCreateResponseDTO);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete]
        [Login]
        [Permissions]
        [Route("{userId}")]
        public async Task<IActionResult> Delete(int userId)
        {
            bool isSuccess = await _userService.DeleteUser(userId);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"User with id {userId} was not found! ");
        }

        [HttpPut]
        [Login]
        [Permissions]
        [Route("{userId}")]
        public async Task<IActionResult> Edit(int userId, UserEditRequestDTO userEditRequestDTO)
        {
            bool isSuccess = await _userService.EditUser(userId, userEditRequestDTO.Username, userEditRequestDTO.Password, userEditRequestDTO.FirstName, userEditRequestDTO.LastName);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"User with id {userId} was not found! ");
        }

        [HttpGet]
        [Login]
        public async Task<IActionResult> GetAll()
        {
            List<UserGetAllResponseDTO> usersDTO = new List<UserGetAllResponseDTO>();

            foreach (User user in await _userService.GetAllUsers())
            {
                usersDTO.Add(new UserGetAllResponseDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Role = user.Role.ToString(),
                });
            }

            return Ok(usersDTO);
        }

        private void MapUser(User userFromDb, UserCreateResponseDTO userCreateResponseDTO)
        {
            userCreateResponseDTO.Id = userFromDb.Id;
            userCreateResponseDTO.Username = userFromDb.Username;
            userCreateResponseDTO.FirstName = userFromDb.FirstName;
            userCreateResponseDTO.LastName = userFromDb.LastName;
            userCreateResponseDTO.Role = userFromDb.Role.ToString();
            userCreateResponseDTO.Team = userFromDb.Teams.Select(x => x.Name).FirstOrDefault();
        }
    }
}


