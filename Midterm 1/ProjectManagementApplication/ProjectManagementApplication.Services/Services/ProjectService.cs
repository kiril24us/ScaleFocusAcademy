using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;
        private readonly ITeamRepository _teamRepository;

        public ProjectService(IProjectRepository projectRepository, ITeamRepository teamRepository)
        {
            _projectRepository = projectRepository;
            _teamRepository = teamRepository;
        }

        public async Task<Tuple<Messages,int>> CreateProject(string name, int userId)
        {
            if (await _projectRepository.GetProjectByName(name) != null)
            {
                return new Tuple<Messages, int>(Messages.ProjectAlreadyExist, 0);
            }

            Project project = new Project
            {
                Name = name,
                IsActive = true,
                UserId = userId
            };

            bool isSuccess = await _projectRepository.Create(project);

            if(isSuccess)
            {
                return new Tuple<Messages, int>(Messages.Success, project.Id);
            }

            return new Tuple<Messages, int>(Messages.OperationWasNotSuccessful, 0);
        }

        public async Task<bool> DeleteProject(int projectId, int userId)
        {
            return await _projectRepository.Delete(projectId, userId);
        }

        public async Task<bool> EditProject(int projectId, string name, int userId)
        {
            Project projectToEdit = await _projectRepository.GetProjectById(projectId, userId);

            if (projectToEdit == null)
            {
                return false;
            }

            projectToEdit.Name = name;

            return await _projectRepository.Edit();
        }

        public async Task<Messages> AssignProject(int projectId, int teamId, int userId)
        {
            Project project = await _projectRepository.GetProjectById(projectId, userId);
            Team team = await _teamRepository.GetTeamById(teamId);

            if (project == null)
            {
                return Messages.ProjectNotFound;
            }
            if (team == null)
            {
                return Messages.TeamNotFound;
            }

            team.ProjectId = projectId;

            await _projectRepository.Edit();

            return Messages.Success;
        }

        public async Task<List<Project>> GetAllProjects(int userId)
        {
            return await _projectRepository.GetAll(userId);
        }

        public async Task<Project> GetProjectByName(string name)
        {
            return await _projectRepository.GetProjectByName(name);
        }

        public async Task<Project> GetProjectById(int projectId, int userId)
        {
            return await _projectRepository.GetProjectById(projectId, userId);
        }
    }
}
