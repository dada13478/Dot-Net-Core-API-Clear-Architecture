namespace Dotnet_beginner_api.Domain.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<List<T>> GetAll();
    Task<T?> GetById(int id);
    Task Add(T entity);
    Task Delete(T entity);
    Task SaveChanges();
}
