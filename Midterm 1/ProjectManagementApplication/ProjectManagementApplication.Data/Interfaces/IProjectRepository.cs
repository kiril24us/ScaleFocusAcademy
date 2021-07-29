using ProjectManagementApplication.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Task<bool> Delete(int projectId, int userId);

        Task<Project> GetProjectByName(string name);

        Task<Project> GetProjectById(int projectId, int userId);

        Task<List<Project>> GetAll(int userId);
    }
}
