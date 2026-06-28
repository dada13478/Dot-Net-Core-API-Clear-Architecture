using Microsoft.EntityFrameworkCore;

public class Old_ProductService //: IProductService
{
    private readonly AppDbContext _db;

    public Old_ProductService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Product>> GetAll()
        => await _db.Products.ToListAsync();

    public async Task<Product?> GetById(int id)
        => await _db.Products.FirstOrDefaultAsync(p => p.Id == id);

    public async Task<Product> Create(Product product)
    {
        _db.Products.Add(product);
        await _db.SaveChangesAsync();
        return product;
    }

    public async Task<bool> Delete(int id)
    {
        var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id);
        if (product == null) return false;
        _db.Products.Remove(product);
        await _db.SaveChangesAsync();
        return true;
    }
}