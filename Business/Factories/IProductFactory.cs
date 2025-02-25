using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;

public interface IProductFactory : IBaseFactory<Product, CreateProductForm>
{
}
