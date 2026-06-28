using Dotnet_beginner_api.Domain.Interfaces;
using Dotnet_beginner_api.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Dotnet_beginner_api.Infrastructure.Repositories;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _db;
    private readonly DbSet<T> _dbSet;

    public GenericRepository(AppDbContext db)
    {
        _db = db;
        _dbSet = db.Set<T>();
    }

    public async Task<List<T>> GetAll()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetById(int id)
        => await _dbSet.FindAsync(id);

    public async Task Add(T entity)
        => await _dbSet.AddAsync(entity);

    public Task Delete(T entity)
    {
        _dbSet.Remove(entity);
        return Task.CompletedTask;
    }

    public async Task SaveChanges()
        => await _db.SaveChangesAsync();
}
