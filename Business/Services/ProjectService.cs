using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IProjectFactory projectFactory, IProjectMapper projectMapper, IProductRepository productRepository, IProductMapper productMapper, IStatusTypeRepository statusTypeRepository) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProjectFactory _projectFactory = projectFactory;
    private readonly IProjectMapper _projectMapper = projectMapper;
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProductMapper _productMapper = productMapper;
    private readonly IStatusTypeRepository _statusTypeRepository = statusTypeRepository;

    public async Task<bool?> AddAsync(CreateProjectForm form)
    {
        Project projectModel = _projectFactory.FromForm(form);
        StatusTypeEntity status = await _statusTypeRepository.GetAsync(x =>x.StatusName == form.Status);
        List<ProductEntity> products = await GetAssociatedEntitiesAsync(projectModel);

        ProjectEntity projectEntity = _projectMapper.ToEntity(projectModel, products, status.Id);
        return await _projectRepository.AddAsync(projectEntity);
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
        ProjectEntity? projectEntity = await _projectRepository.GetAsync(x => x.Id == id);
        if (projectEntity == null)
        {
            return null;
        }
        List<ProductReferenceModel> associatedProductsReferences = TransformEntitesToReferenceModels(projectEntity.Products);
        Project project = _projectMapper.ToModel(projectEntity, associatedProductsReferences);
        return project;
    }

    public async Task<Project?> GetByTitleAsync(string title)
    {
        ProjectEntity? projectEntity = await _projectRepository.GetAsync(x => x.Title == title);
        if (projectEntity == null)
        {
            return null;
        }
        List<ProductReferenceModel> associatedProductsReferences = TransformEntitesToReferenceModels(projectEntity.Products);
        Project project = _projectMapper.ToModel(projectEntity, associatedProductsReferences);
        return project;
    }

    public async Task<bool?> UpdateAsync(UpdateProjectForm form, Project existingProject)
    {
        existingProject.Title = form.Title;
        existingProject.Description = form.Description;
        existingProject.StartDate = form.StartDate;
        existingProject.EndDate = form.EndDate;
        existingProject.AssociatedUser = form.AssociatedUser;
        existingProject.AssociatedCustomer = form.AssociatedCustomer;
        existingProject.AssociatedProducts = form.AssociatedProducts;

        List<ProductEntity> products = await GetAssociatedEntitiesAsync(existingProject);
        StatusTypeEntity status = await _statusTypeRepository.GetAsync(x => x.StatusName == form.Status);


        var updatedEntity = _projectMapper.ToEntity(existingProject, products, status.Id);

        return await _projectRepository.UpdateAsyncc(updatedEntity, products);
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        return await _projectRepository.DeleteAsync(x => x.Id == id);
    }

    private async Task<List<ProductEntity>> GetAssociatedEntitiesAsync(Project projectModel)
    {
        List<ProductEntity> products = [];
        foreach (var product in projectModel.AssociatedProducts)
        {
            ProductEntity productEntity = await _productRepository.GetProductByIdAsync(product.Id);
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
}
