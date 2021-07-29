namespace ToDoApplication.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        bool Create(TEntity entityToCreate);

        bool Delete(TEntity entityToDelete);

        bool Edit();

    }
}
