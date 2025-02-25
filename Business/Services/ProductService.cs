using Domain.Dtos;
using Domain.Models;

namespace Business.Services;

public class ProductService : IProductService
{
    public Task<bool> AddAsync(CreateProductForm form)
    {
        throw new NotImplementedException();
    }

    public Task<bool?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Product>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<Product?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<Product?> UpdateAsync(int id, UpdateProductForm form)
    {
        throw new NotImplementedException();
    }
}
