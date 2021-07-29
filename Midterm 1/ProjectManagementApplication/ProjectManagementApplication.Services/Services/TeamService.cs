using ProjectManagementApplication.Data.Entities;
using ProjectManagementApplication.Data.Interfaces;
using ProjectManagementApplication.Services.Enums;
using ProjectManagementApplication.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Services.Services
{
    public class TeamService : ITeamService
    {
        private readonly ITeamRepository _teamRepository;
        private readonly IUserRepository _userRepository;

        public TeamService(ITeamRepository teamRepository, IUserRepository userRepository)
        {
            _teamRepository = teamRepository;
            _userRepository = userRepository;
        }

        public async Task<Messages> AssignUser(int userId, int teamId)
        {
            User user = await _userRepository.GetUserById(userId);
            Team team = await _teamRepository.GetTeamById(teamId);

            if(user == null)
            {
                return Messages.UserNotFound;
            }
            if(team == null)
            {
                return Messages.TeamNotFound;
            }

            user.Teams.Add(team);

            await _teamRepository.Edit();

            return Messages.Success;
        }

        public async Task<Tuple<Messages, int>> CreateTeam(string name)
        {
            if (await _teamRepository.GetTeamByName(name) != null)
            {
                return new Tuple<Messages, int>(Messages.TeamAlreadyExist, 0);
            }

            Team team = new Team
            {
                Name = name,
                IsActive = true
            };

            bool isSuccess = await _teamRepository.Create(team);

            if (isSuccess)
            {
                return new Tuple<Messages, int>(Messages.Success, team.Id);
            }

            return new Tuple<Messages, int>(Messages.OperationWasNotSuccessful, 0);
        }

        public async Task<bool> DeleteTeam(int teamId)
        {
            Team team = await _teamRepository.GetTeamById(teamId);

            if (team == null)
            {
                return false;
            }

            team.IsActive = false;

            return await _teamRepository.Edit();
        }

        public async Task<bool> EditTeam(int teamId, string name)
        {
            Team teamToEdit = await _teamRepository.GetTeamById(teamId);

            if (teamToEdit == null)
            {
                return false;
            }

            teamToEdit.Name = name;

            return await _teamRepository.Edit();
        }

        public async Task<List<Team>> GetAllTeams()
        {
            return await _teamRepository.GetAll();
        }

        public async Task<Team> GetTeamById(int teamId)
        {
            return await _teamRepository.GetTeamById(teamId);
        }
    }
}
