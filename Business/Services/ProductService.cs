using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Business.Services;

public class ProductService(IProductRepository productRepository, IProductFactory productFactory, IProductMapper productMapper, IProjectRepository projectRepository, IProjectMapper projectMapper) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProductFactory _productFactory = productFactory;
    private readonly IProductMapper _productMapper = productMapper;
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProjectMapper _projectMapper = projectMapper;
    public async Task<bool?> AddAsync(CreateProductForm form)
    {
        Product productModel = _productFactory.FromForm(form);

        List<ProjectEntity> projects = await GetAssociatedEntitiesAsync(productModel);

        ProductEntity productEntity = _productMapper.ToEntity(productModel, projects);
        return await _productRepository.AddAsync(productEntity);
    }


    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        List<Product> productList = [];
        var result = await _productRepository.GetAllAsync(query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );


        foreach (var productEntity in result)
        {
            List<ProjectReferenceModel> associatedProjectReferences = TransformEntitiesToReferenceModels(productEntity.Projects);
            productList.Add(_productMapper.ToModel(productEntity, associatedProjectReferences));
        }
        return productList;
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var productEntity = await _productRepository.GetAsync(
        x => x.Id == id,
         query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );
        if (productEntity == null)
        {
            return null;
        }
        List<ProjectReferenceModel> associatedProjectReferences = TransformEntitiesToReferenceModels(productEntity.Projects);

        Product product = _productMapper.ToModel(productEntity, associatedProjectReferences);
        return product;
    }

    public async Task<bool?> UpdateAsync(UpdateProductForm form, Product existingProduct)
    {
        existingProduct.ProductName = form.ProductName;
        existingProduct.Price = Decimal.Parse(form.InputPrice);


        List<ProjectEntity> projects = await GetAssociatedEntitiesAsync(existingProduct);

        var updatedEntity = _productMapper.ToEntity(existingProduct, projects);

        return await _productRepository.UpdateAsync(x => x.Id == existingProduct.Id, updatedEntity);
    }
    public async Task<bool?> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(x => x.Id == id);
    }

    private async Task<List<ProjectEntity>> GetAssociatedEntitiesAsync(Product productModel)
    {
        List<ProjectEntity> projects = [];
        foreach (var project in productModel.AssociatedProjects)
        {
            ProjectEntity projectEntity = await _projectRepository.GetProjectByIdAsync(project.Id);
            if (projectEntity != null)
            {
                projects.Add(projectEntity);
            }
        }

        return projects;
    }

    private List<ProjectReferenceModel> TransformEntitiesToReferenceModels(ICollection<ProjectEntity> projectEntities)
    {
        List<ProjectReferenceModel> projects = [];
        foreach (var project in projectEntities)
        {
            projects.Add(_projectMapper.ToReferenceModel(project));
        }

        return projects;
    }
}

