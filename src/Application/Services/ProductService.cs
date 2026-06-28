using Dotnet_beginner_api.Application.DTOs;
using Dotnet_beginner_api.Application.Interfaces;
using Dotnet_beginner_api.Domain.Entities;
using Dotnet_beginner_api.Domain.Interfaces;

namespace Dotnet_beginner_api.Application.Services;

public class ProductService : IProductService
{
    private readonly IGenericRepository<Product> _repo;

    public ProductService(IGenericRepository<Product> repo)
    {
        _repo = repo;
    }

    public async Task<List<ProductDto>> GetAll()
    {
        var products = await _repo.GetAll();
        return products.Select(ToDto).ToList();
    }

    public async Task<ProductDto?> GetById(int id)
    {
        var product = await _repo.GetById(id);
        return product == null ? null : ToDto(product);
    }

    public async Task<ProductDto> Create(CreateProductDto dto)
    {
        var product = new Product { Name = dto.Name, Price = dto.Price };
        await _repo.Add(product);
        await _repo.SaveChanges();
        return ToDto(product);
    }

    public async Task<bool> Update(int id, UpdateProductDto dto)
    {
        var existing = await _repo.GetById(id);
        if (existing == null) return false;
        existing.Name = dto.Name;
        existing.Price = dto.Price;
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

    private static ProductDto ToDto(Product p) => new()
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price
    };
}
