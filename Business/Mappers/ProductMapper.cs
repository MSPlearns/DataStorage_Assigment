using Business.Services;
using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public class ProductMapper(IProjectMapper projectMapper, IProductService productService) : IProductMapper
{
    private readonly IProjectMapper _projectMapper = projectMapper;
    private readonly IProductService _productService = productService;
    public async Task<ProductEntity> ToEntity(Product model)
    {
        //The use of WhenAll was suggested by AI. It is used to execute multiple tasks concurrently and wait until they are completed before moving on.
        var projects = await Task.WhenAll(model.AssociatedProjects.Select(p => _projectMapper.ToEntity(p)));

        ProductEntity entity = new()
        {
            ProductName = model.ProductName,
            Price = model.Price,
            Projects = projects.ToList()
        };
        if (model.Id != default)
            entity.Id = model.Id;
        return entity;
    }

    public async Task<ProductEntity?> ToEntity(ProductReferenceModel referenceModel)
    {

        Product model = await _productService.GetByIdAsync(referenceModel.Id);
        if (model == null)
        {
            return null;
        }
        ProductEntity entity = await ToEntity(model);
        return entity;
    }

    public Product ToModel(ProductEntity entity)
    {
        return new Product
        {
            Id = entity.Id,
            ProductName = entity.ProductName,
            Price = entity.Price,
            AssociatedProjects = entity.Projects.Select(_projectMapper.ToReferenceModel).ToList()
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
}
