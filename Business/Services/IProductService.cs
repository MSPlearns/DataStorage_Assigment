using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services
{
    public interface IProductService
    {
        Task AddAsync(CreateProductForm form);
        Task<bool> AlreadyExists(Expression<Func<ProductEntity, bool>> predicate);
        Task<bool?> DeleteAsync(int id);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateProductForm form, Product existingProduct);
    }
}