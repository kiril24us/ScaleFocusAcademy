using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Api.Attributes;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.DTO.Requests.ProjectRequests;
using ProjectManagementApplication.DTO.Responses.ProjectResponses;
using ProjectManagementApplication.Services.Auth;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Api.Controllers
{
    [Route("api/[controller]/[action]/")]
    [ApiController]
    public class ProjectsController : Controller
    {
        private readonly IProjectService _projectService;
        private readonly IAuthentication _authentication;

        public ProjectsController(IProjectService projectService, IAuthentication authentication)
        {
            _projectService = projectService;
            _authentication = authentication;
        }

        [HttpPost]
        [Login]
        public async Task<IActionResult> Create(ProjectCreateRequestDTO projectCreateRequestDTO)
        {
            Tuple<Messages, int> tuple =  await _projectService.CreateProject(projectCreateRequestDTO.Name, Authentication.CurrentUser.Id);

            switch (tuple.Item1)
            {
                case Messages.ProjectAlreadyExist:
                    return BadRequest($"Already Exist Project with name {projectCreateRequestDTO.Name}");

                case Messages.OperationWasNotSuccessful:
                    return BadRequest($"Operation was no successful");

                case Messages.Success:
                    Project projectFromDb = await _projectService.GetProjectById(tuple.Item2, Authentication.CurrentUser.Id);
                    return CreatedAtAction("Create", projectFromDb);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete]
        [Login]
        [Route("{projectId}")]
        public async Task<IActionResult> Delete(int projectId)
        {
            bool isSuccess = await _projectService.DeleteProject(projectId, Authentication.CurrentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Project with Id {projectId} was not found or is not created by User with Id {Authentication.CurrentUser.Id} ");
        }

        [HttpPut]
        [Login]
        [Permissions]
        [Route("{projectId}")]
        public async Task<IActionResult> Edit(int projectId, ProjectCreateRequestDTO projectEditRequestDTO)
        {
            bool isSuccess = await _projectService.EditProject(projectId, projectEditRequestDTO.Name, Authentication.CurrentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Project with Id {projectId} was not found or is not created by User with Id {Authentication.CurrentUser.Id}");
        }

        [HttpPost]
        [Login]
        public async Task<IActionResult> AssignProjectToATeam(AssignProjectToATeamRequestDTO assignProjectToATeamRequestDTO)
        {
            switch (await _projectService.AssignProject(assignProjectToATeamRequestDTO.ProjectId, assignProjectToATeamRequestDTO.TeamId, Authentication.CurrentUser.Id))
            {
                case Messages.TeamNotFound:
                    return BadRequest($"Team with Id {assignProjectToATeamRequestDTO.TeamId} was not found");

                case Messages.ProjectNotFound:
                    return BadRequest($"Project with Id {assignProjectToATeamRequestDTO.ProjectId} was not found or is not created by User with Id {Authentication.CurrentUser.Id}");

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
            List<ProjectGetAllResponseDTO> projectsDTO = new List<ProjectGetAllResponseDTO>();

            foreach (Project project in await _projectService.GetAllProjects(Authentication.CurrentUser.Id))
            {
                projectsDTO.Add(new ProjectGetAllResponseDTO
                {
                    Id = project.Id,
                    Name = project.Name,
                    UserId = project.UserId
                });
            }

            return Ok(projectsDTO);
        }
    }
}
