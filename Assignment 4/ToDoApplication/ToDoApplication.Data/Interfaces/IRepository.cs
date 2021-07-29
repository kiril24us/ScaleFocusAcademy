using System.Collections.Generic;

namespace ToDoApplication.Data.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool Create(TEntity entityToCreate);

        bool Delete(TEntity entityToDelete);

        bool Edit();

        List<TEntity> GetAll(int id);
    }
}
