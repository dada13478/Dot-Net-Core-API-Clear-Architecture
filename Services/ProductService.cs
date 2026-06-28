public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repo;

    public ProductService(IGenericRepository<Product> repo)
    {
        _repo = repo;
    }

    public async Task<List<Product>> GetAll()
        => await _repo.GetAll();

    public async Task<Product?> GetById(int id)
        => await _repo.GetById(id);

    public async Task<Product> Create(Product product)
    {
        await _repo.Add(product);
        await _repo.SaveChanges();
        return product;
    }

    public async Task<bool> Update(int id, Product product)
    {
        var existing = await _repo.GetById(id);
        if (existing == null) return false;
        existing.Name = product.Name;
        existing.Price = product.Price;
        await _repo.SaveChanges();
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var existing = await _repo.GetById(id);
        if (existing == null) return false;
        await _repo.Delete(existing);
        await _repo.SaveChanges();
        return true;
    }
}