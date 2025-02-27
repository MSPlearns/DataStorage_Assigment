using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Business.Mappers;

public class ProjectMapper(IUserMapper userMapper, 
                           IProductMapper productMapper, 
                           ICustomerMapper customerMapper, 
                           IStatusTypeRepository statusTypeRepository,
                           IProjectService projectService) 
    : IProjectMapper
{
    private readonly IUserMapper _userMapper = userMapper;
    private readonly IProductMapper _productMapper = productMapper;
    private readonly ICustomerMapper _customerMapper= customerMapper;
    private readonly IStatusTypeRepository _statusTypeRepository = statusTypeRepository;
    private readonly IProjectService _projectService = projectService;
    public async Task<ProjectEntity> ToEntity(Project model)
    {
        //The use of WhenAll was suggested by AI. It is used to execute multiple tasks concurrently and wait until they are completed before moving on.
        var products = await Task.WhenAll(model.AssociatedProducts.Select(async product => await _productMapper.ToEntity(product)));

        var status = await _statusTypeRepository.GetAsync(x => x.StatusName == model.Status);


        ProjectEntity entity = new() 
        {
            Title = model.Title,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            UserId = model.AssociatedUser.Id,
            CustomerId = model.AssociatedCustomer.Id,
            StatusId = status!.Id,
            Products = products.ToList()!
        };

        if (model.Id != default)
            entity.Id = model.Id;
        return entity;
    }

    public async Task<ProjectEntity?> ToEntity(ProjectReferenceModel referenceModel)
    {
        Project model = await _projectService.GetByIdAsync(referenceModel.Id);
        if (model == null)
        {
            return null;
        }
        ProjectEntity entity = await ToEntity(model);
        return entity;
    }

    public Project ToModel(ProjectEntity entity)
    {
        Project project = new()
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate,
            AssociatedUser = _userMapper.ToReferenceModel(entity.User),
            AssociatedCustomer = _customerMapper.ToReferenceModel(entity.Customer),
            Status = entity.Status.StatusName,
            AssociatedProducts = entity.Products.Select(_productMapper.ToReferenceModel).ToList()
        };
        return project;
    }

    public ProjectReferenceModel ToReferenceModel(ProjectEntity entity)
    {
        return new ProjectReferenceModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Status = entity.Status.StatusName
        };
    }
}
