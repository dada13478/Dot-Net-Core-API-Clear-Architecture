using Dotnet_beginner_api.Application.DTOs;

namespace Dotnet_beginner_api.Application.Interfaces;

public interface IProductService
{
    Task<List<ProductDto>> GetAll();
    Task<ProductDto?> GetById(int id);
    Task<ProductDto> Create(CreateProductDto dto);
    Task<bool> Update(int id, UpdateProductDto dto);
    Task<bool> Delete(int id);
}
