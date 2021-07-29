using IdentityServer.Data.Entities;
using IdentityServer.DTO.Requests;
using IdentityServer.DTO.Responses;
using IdentityServer.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IdentityServer.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Policy = "AdminRoleOnly")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
            : base()
        {
            _userService = userService;
        }
        
        // POST api/Users 
        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDTO user)
        {
            bool result = await _userService.CreateUser(user.UserName, user.Password, user.Role);

            if (result)
            {
                User userFromDB = await _userService.GetUserByName(user.UserName);

                return CreatedAtAction("Get", "Users", new { id = userFromDB.Id }, null);
            }

            return BadRequest();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<UserResponseDTO> Get(string id)
        {
            User userFromDB = await _userService.GetUserById(id);
            return new UserResponseDTO()
            {
                UserName = userFromDB.UserName,
                Id = userFromDB.Id,
            };
        }

        [HttpGet]
        [Route("All")]
        public async Task<List<UserResponseDTO>> GetAll()
        {
            List<UserResponseDTO> users = new List<UserResponseDTO>();

            foreach (var user in await _userService.GetAll())
            {
                users.Add(new UserResponseDTO()
                {
                    Id = user.Id,
                    CreatedAt = user.CreatedAt,
                    UserName = user.UserName
                });
            }
            return users;
        }
    }
}
