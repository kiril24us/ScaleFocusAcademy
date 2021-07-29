using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Services.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface IProjectService
    {
        Task<Tuple<Messages,int>> CreateProject(string name, int userId);

        Task<bool> DeleteProject(int projectId, int userId);

        Task<bool> EditProject(int projectId, string name, int userId);

        Task<Messages> AssignProject(int projectId, int teamId, int userId);

        Task<List<Project>> GetAllProjects(int userId);

        Task<Project> GetProjectByName(string name);

        Task<Project> GetProjectById(int projectId, int userId);
    }
}
