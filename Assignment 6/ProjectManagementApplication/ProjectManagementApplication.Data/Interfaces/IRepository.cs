using System.Threading.Tasks;

namespace ProjectManagementApplication.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<bool> Create(TEntity entityToCreate);

        Task<bool> Edit();
    }
}
