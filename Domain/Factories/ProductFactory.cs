using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;
public class ProductFactory : IProductFactory
{
    public Product FromForm(CreateProductForm form)
    {
        return new Product
        {
            ProductName = form.ProductName,
            Price = Decimal.Parse(form.InputPrice)
        };
    }

    public Product FromForm(UpdateProductForm form)
    {
        return new Product
        {
            ProductName = form.ProductName,
            Price = Decimal.Parse(form.InputPrice)
        };
    }
}
