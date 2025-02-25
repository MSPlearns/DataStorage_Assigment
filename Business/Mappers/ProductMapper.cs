using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public class ProductMapper(IProjectMapper projectMapper) : IProductMapper
{
    private readonly IProjectMapper _projectMapper = projectMapper;
    public ProductEntity ToEntity(Product model)
    {
        ProductEntity entity = new()
        {
            ProductName = model.ProductName,
            Price = model.Price,
            Projects = model.Projects.Select(_projectMapper.ToEntity).ToList()
        };
        if (model.Id != default)
            entity.Id = model.Id;
        return entity;
    }

    public Product ToModel(ProductEntity entity)
    {
        return new Product
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            Price = entity.Price,
            Projects = entity.Projects.Select(_projectMapper.ToModel).ToList()
        };
    }
}
