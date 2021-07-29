using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.DTO.Requests.ProjectRequests;
using ProjectManagementApplication.DTO.Responses.ProjectResponses;
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
        private readonly IUserManager _userManager;

        public ProjectsController(IProjectService projectService, IUserManager userManager)
        {
            _projectService = projectService;
            _userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(ProjectCreateRequestDTO projectCreateRequestDTO)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);
            
            Tuple<Messages, int> tuple =  await _projectService.CreateProject(projectCreateRequestDTO.Name, currentUser.Id);

            switch (tuple.Item1)
            {
                case Messages.ProjectAlreadyExist:
                    return BadRequest($"Already Exist Project with name {projectCreateRequestDTO.Name}");

                case Messages.OperationWasNotSuccessful:
                    return BadRequest($"Operation was no successful");

                case Messages.Success:
                    Project projectFromDb = await _projectService.GetProjectById(tuple.Item2, currentUser.Id);
                    return CreatedAtAction("Create", projectFromDb);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpDelete]
        [Authorize(Policy = "CreatorOnly")]
        [Route("{projectId}")]
        public async Task<IActionResult> Delete(int projectId)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            bool isSuccess = await _projectService.DeleteProject(projectId, currentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Project with Id {projectId} was not found or is not created by User with Id {""} ");
        }

        [HttpPut]
        [Authorize(Policy = "CreatorOnly")]
        [Route("{projectId}")]
        public async Task<IActionResult> Edit(int projectId, ProjectCreateRequestDTO projectEditRequestDTO)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            bool isSuccess = await _projectService.EditProject(projectId, projectEditRequestDTO.Name, currentUser.Id);

            if (isSuccess)
            {
                return StatusCode(200);
            }

            return BadRequest($"Project with Id {projectId} was not found or is not created by User with Id {""}");
        }

        [HttpPost]
        [Authorize(Policy = "CreatorOnly")]
        public async Task<IActionResult> AssignProjectToATeam(AssignProjectToATeamRequestDTO assignProjectToATeamRequestDTO)
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            switch (await _projectService.AssignProject(assignProjectToATeamRequestDTO.ProjectId, assignProjectToATeamRequestDTO.TeamId, currentUser.Id))
            {
                case Messages.TeamNotFound:
                    return BadRequest($"Team with Id {assignProjectToATeamRequestDTO.TeamId} was not found");

                case Messages.ProjectNotFound:
                    return BadRequest($"Project with Id {assignProjectToATeamRequestDTO.ProjectId} was not found or is not created by User with Id {""}");

                case Messages.Success:
                    return StatusCode(200);

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetMine()
        {
            User currentUser = await _userManager.GetTheUserAsync(HttpContext.User);

            List<ProjectGetAllResponseDTO> projectsDTO = new List<ProjectGetAllResponseDTO>();

            foreach (Project project in await _projectService.GetMineProjects(currentUser.Id))
            {
                MapDTOWithProject(projectsDTO, project);
            }

            return Ok(projectsDTO);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            List<ProjectGetAllResponseDTO> projectsDTO = new List<ProjectGetAllResponseDTO>();

            foreach (Project project in await _projectService.GetAllProjects())
            {
                MapDTOWithProject(projectsDTO, project);
            }

            return Ok(projectsDTO);
        }

        private static void MapDTOWithProject(List<ProjectGetAllResponseDTO> projectsDTO, Project project)
        {
            projectsDTO.Add(new ProjectGetAllResponseDTO
            {
                Id = project.Id,
                Name = project.Name,
                UserId = project.UserId
            });
        }
    }
}
