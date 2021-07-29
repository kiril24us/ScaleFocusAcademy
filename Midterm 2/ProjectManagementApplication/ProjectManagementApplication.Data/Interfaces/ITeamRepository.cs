using ProjectManagementApplication.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface ITeamRepository : IRepository<Team>
    {
        Task<Team> GetTeamByName(string name);

        Task<Team> GetTeamById(int teamId);

        Task<List<Team>> GetAll();
    }
}
