using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Services.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Interfaces
{
    public interface ITeamService
    {
        Task<Tuple<Messages,int>> CreateTeam(string name);

        Task<bool> DeleteTeam(int teamId);

        Task<Messages> AssignUser(int userId, int teamId);

        Task<Team> GetTeamById(int teamId);

        Task<bool> EditTeam(int teamId, string name);

        Task<List<Team>> GetAllTeams();
    }
}
