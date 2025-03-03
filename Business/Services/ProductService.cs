using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.Services;

public class ProductService(IProductRepository productRepository,
                            IProductFactory productFactory,
                            IProductMapper productMapper,
                            IProjectMapper projectMapper) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProductFactory _productFactory = productFactory;
    private readonly IProductMapper _productMapper = productMapper;
    private readonly IProjectMapper _projectMapper = projectMapper;
    public async Task AddAsync(CreateProductForm form)
    {
        await _productRepository.BeginTransactionAsync();
        try
        {
            Product productModel = _productFactory.FromForm(form);
            ProductEntity productEntity = _productMapper.ToEntity(productModel);
            await _productRepository.AddAsync(productEntity);
            await _productRepository.SaveAsync();
            await _productRepository.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _productRepository.RollbackTransactionAsync();
            throw;
        }
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
        ProductEntity? productEntity = await _productRepository.GetAsync(
        x => x.Id == id,
         query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );

        if (productEntity == null)
            return null;

        List<ProjectReferenceModel> associatedProjectReferences = TransformEntitiesToReferenceModels(productEntity.Projects);
        Product product = _productMapper.ToModel(productEntity, associatedProjectReferences);
        return product;
    }

    public async Task UpdateAsync(UpdateProductForm form, Product existingProduct)
    {
        await _productRepository.BeginTransactionAsync();
        try
        {
            existingProduct.ProductName = form.ProductName;
            existingProduct.Price = Decimal.Parse(form.InputPrice);

            var updatedEntity = _productMapper.ToEntity(existingProduct);
            await _productRepository.UpdateAsync(x => x.Id == existingProduct.Id, updatedEntity);
            await _productRepository.SaveAsync();
            await _productRepository.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _productRepository.RollbackTransactionAsync();
            throw;
        }
    }
    public async Task<bool?> DeleteAsync(int id)
    {
        await _productRepository.BeginTransactionAsync();
        try
        {
            ProductEntity? productEntity = await _productRepository.GetAsync(x => x.Id == id);
             _productRepository.Delete(productEntity!);
            await _productRepository.SaveAsync();
            await _productRepository.CommitTransactionAsync();
            return true;
        }
        catch (Exception)
        {
            await _productRepository.RollbackTransactionAsync();
            throw;
        }
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

    public async Task<bool> AlreadyExists(Expression<Func<ProductEntity, bool>> predicate)
    {
        return await _productRepository.EntityExistsAsync(predicate);
    }
}

