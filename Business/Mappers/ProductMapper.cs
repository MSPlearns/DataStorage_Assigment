using Business.Services;
using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public class ProductMapper : IProductMapper
{
    
    public ProductEntity ToEntity(Product model, List<ProjectEntity> projectEntities)
    {
        ProductEntity entity = new()
        {
            ProductName = model.ProductName,
            Price = model.Price,
            Projects = projectEntities
        };
        if (model.Id != default)
            entity.Id = model.Id;
        return entity;
    }


    public Product ToModel(ProductEntity entity, List<ProjectReferenceModel> projectReferenceModels)
    {
        return new Product
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            Price = entity.Price,
            AssociatedProjects = projectReferenceModels
        };
    }

    public ProductReferenceModel ToReferenceModel(ProductEntity entity)
    {
        return new ProductReferenceModel
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            Price = entity.Price,
        };
    }

    public ProductReferenceModel ToReferenceModel(Product model)
    {
        return new ProductReferenceModel
        {
            Id = model.Id,
            ProductName = model.ProductName,
            Price = model.Price,
        };
    }
}
