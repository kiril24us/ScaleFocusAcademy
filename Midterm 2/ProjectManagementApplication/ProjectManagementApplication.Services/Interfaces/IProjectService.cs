using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Services.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Tuple<Messages,int>> CreateProject(string name, string userId);

        Task<bool> DeleteProject(int projectId, string userId);

        Task<bool> EditProject(int projectId, string name, string userId);

        Task<Messages> AssignProject(int projectId, int teamId, string userId);

        Task<List<Project>> GetAllProjects();

        Task<List<Project>> GetMineProjects(string userId);

        Task<Project> GetProjectByName(string name);

        Task<Project> GetProjectById(int projectId, string userId);
    }
}
