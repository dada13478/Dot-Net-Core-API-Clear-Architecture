using Microsoft.EntityFrameworkCore;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly AppDbContext _db;// 整個資料庫
    private readonly DbSet<T> _dbSet;// 其中一張資料表

    public GenericRepository(AppDbContext db)
    {
        _db = db;// 整個資料庫
        _dbSet = db.Set<T>();// 從資料庫取出 T 對應的資料表
    }

    public async Task<List<T>> GetAll()
        => await _dbSet.ToListAsync();

    public async Task<T?> GetById(int id)
        => await _dbSet.FindAsync(id);

    public async Task Add(T entity)
        => await _dbSet.AddAsync(entity);

    public async Task Delete(T entity)
        => _dbSet.Remove(entity);

    public async Task SaveChanges()
        => await _db.SaveChangesAsync();
}