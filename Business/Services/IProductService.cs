using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Services;

public interface IProductService : IBaseService<ProductEntity, Product, CreateProductForm, UpdateProductForm>
{
}
