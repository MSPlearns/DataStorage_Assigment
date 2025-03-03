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

public class ProjectService(IProjectRepository projectRepository,
                            IProjectFactory projectFactory,
                            IProjectMapper projectMapper,
                            IProductService productService,
                            IProductMapper productMapper,
                            IStatusTypeRepository statusTypeRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProjectFactory _projectFactory = projectFactory;
    private readonly IProjectMapper _projectMapper = projectMapper;
    private readonly IProductService _productService = productService;
    private readonly IProductMapper _productMapper = productMapper;
    private readonly IStatusTypeRepository _statusTypeRepository = statusTypeRepository;

    public async Task AddAsync(CreateProjectForm form)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            Project projectModel = _projectFactory.FromForm(form);
            StatusTypeEntity? status = await _statusTypeRepository.GetAsync(x => x.StatusName == form.Status);
            List<ProductEntity> products = await GetAssociatedEntitiesAsync(projectModel);

            ProjectEntity projectEntity = _projectMapper.ToEntity(projectModel, products, status!.Id);
            await _projectRepository.AddAsync(projectEntity);
            await _projectRepository.SaveAsync();
            await _projectRepository.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _projectRepository.RollbackTransactionAsync();
        }
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        List<Project> projectList = [];
        var result = await _projectRepository.GetAllAsync(query => query
        .Include(p => p.User)
        .Include(p => p.Customer)
        .Include(p => p.Status)
        .Include(p => p.Products)
        );
        foreach (var projectEntity in result)
        {
            List<ProductReferenceModel> associatedProductsReferences = TransformEntitesToReferenceModels(projectEntity.Products);
            projectList.Add(_projectMapper.ToModel(projectEntity, associatedProductsReferences));
        }
        return projectList;
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        ProjectEntity? projectEntity = await _projectRepository.GetAsync(x => x.Id == id,
        query => query
        .Include(p => p.User)
        .Include(p => p.Customer)
        .Include(p => p.Status)
        .Include(p => p.Products)
        );
        if (projectEntity == null)
            return null;

        List<ProductReferenceModel> associatedProductsReferences = TransformEntitesToReferenceModels(projectEntity.Products);
        Project project = _projectMapper.ToModel(projectEntity, associatedProductsReferences);
        return project;
    }

    public async Task UpdateAsync(UpdateProjectForm form, Project existingProject)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            existingProject.Title = form.Title;
            existingProject.Description = form.Description;
            existingProject.StartDate = form.StartDate;
            existingProject.EndDate = form.EndDate;
            existingProject.AssociatedUser = form.AssociatedUser;
            existingProject.AssociatedCustomer = form.AssociatedCustomer;
            existingProject.AssociatedProducts = form.AssociatedProducts;

            List<ProductEntity> products = await GetAssociatedEntitiesAsync(existingProject);
            StatusTypeEntity? status = await _statusTypeRepository.GetAsync(x => x.StatusName == form.Status);

            var updatedEntity = _projectMapper.ToEntity(existingProject, products, status!.Id);
            await _projectRepository.UpdateAsync(updatedEntity, products);
            //await _projectRepository.SaveAsync();
            await _projectRepository.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _projectRepository.RollbackTransactionAsync();
        }
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        await _projectRepository.BeginTransactionAsync();
        try
        {
            ProjectEntity? projectEntity = await _projectRepository.GetAsync(x => x.Id == id);
            _projectRepository.Delete(projectEntity!);
            await _projectRepository.SaveAsync();
            await _projectRepository.CommitTransactionAsync();
            return true;
        }
        catch (Exception)
        {
            await _projectRepository.RollbackTransactionAsync();
            return false;
        }
    }

    private async Task<List<ProductEntity>> GetAssociatedEntitiesAsync(Project projectModel)
    {
        List<ProductEntity> products = [];
        foreach (var product in projectModel.AssociatedProducts)
        {
            Product? productModel = await _productService.GetByIdAsync(product.Id); 
            ProductEntity productEntity = _productMapper.ToEntity(productModel!);
            if (productEntity != null)
            {
                products.Add(productEntity);
            }
        }
        return products;
    }

    private List<ProductReferenceModel> TransformEntitesToReferenceModels(ICollection<ProductEntity> productEntities)
    {
        List<ProductReferenceModel> products = [];
        foreach (var product in productEntities)
        {
            products.Add(_productMapper.ToReferenceModel(product));
        }

        return products;
    }

    public async Task<bool> AlreadyExists(Expression<Func<ProjectEntity, bool>> predicate)
    {
        return await _projectRepository.EntityExistsAsync(predicate);
    }
}
