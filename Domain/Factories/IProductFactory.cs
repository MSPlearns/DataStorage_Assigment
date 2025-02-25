using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;

public interface IProductFactory : IBaseFactory<Product, CreateProductForm, UpdateProductForm>
{
}
