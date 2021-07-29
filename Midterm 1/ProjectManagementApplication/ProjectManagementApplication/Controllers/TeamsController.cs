using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Api.Attributes;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.DTO.Requests.TeamRequests;
using ProjectManagementApplication.DTO.Responses.TeamResponses;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Api.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class TeamsController : Controller
    {
        private readonly ITeamService _teamService;

        public TeamsController(ITeamService teamService)
        {
            _teamService = teamService;
        }

        [HttpPost]
        [Login]
        [Permissions]
        public async Task<IActionResult> Create(TeamBaseRequestDTO team)
        {
            Tuple<Messages,int> tuple = await _teamService.CreateTeam(team.Name);

            switch (tuple.Item1)
            {
                case Messages.TeamAlreadyExist:
                    return BadRequest($"Already Exist Team with name {team.Name}");

                case Messages.Success:
                    Team teamFromDb = await _teamService.GetTeamById(tuple.Item2);
                    return CreatedAtAction("Create", teamFromDb);

                case Messages.OperationWasNotSuccessful:
                    return BadRequest("Operation was not successful");

                case Messages.AlreadyExistWorkLog:
                    return BadRequest("Already exist such Work Log");

                default:
                    throw new ArgumentOutOfRangeException();
            }           
        }

        [HttpDelete]
        [Login]
        [Permissions]
        [Route("{teamId}")]
        public async Task<IActionResult> Delete(int teamId)
        {
            bool isSuccess = await _teamService.DeleteTeam(teamId);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Team with Id {teamId} was not found! ");
        }

        [HttpPut]
        [Login]
        [Permissions]
        [Route("{teamId}")]
        public async Task<IActionResult> Edit(int teamId, TeamBaseRequestDTO teamBaseRequestDTO)
        {
            bool isSuccess = await _teamService.EditTeam(teamId, teamBaseRequestDTO.Name);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Team with Id {teamId} was not found! ");
        }

        [HttpPost]
        [Login]
        [Permissions]
        public async Task<IActionResult> AssignUserToATeam(AssignUserToATeamRequestDTO assignUserToATeamRequestDTO)
        {
            switch (await _teamService.AssignUser(assignUserToATeamRequestDTO.UserId, assignUserToATeamRequestDTO.TeamId))
            {
                case Messages.TeamNotFound:
                    return BadRequest($"Team with Id {assignUserToATeamRequestDTO.TeamId} was not found");

                case Messages.UserNotFound:
                    return BadRequest($"User with Id {assignUserToATeamRequestDTO.UserId} was not found");

                case Messages.Success:
                    return StatusCode(200);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet]
        [Login]
        public async Task<IActionResult> GetAll()
        {
            List<TeamGetAllResponseDTO> teamsDTO = new List<TeamGetAllResponseDTO>();

            foreach (Team team in await _teamService.GetAllTeams())
            {
                teamsDTO.Add(new TeamGetAllResponseDTO
                {
                    Id = team.Id,
                    Name = team.Name,
                });
            }

            return Ok(teamsDTO);
        }
    }
}
