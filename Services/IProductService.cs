public interface IProductService
{
    Task<List<Product>> GetAll();
    Task<Product?> GetById(int id);
    Task<Product> Create(Product product);
    Task<bool> Update(int id, Product product);
    Task<bool> Delete(int id);
}