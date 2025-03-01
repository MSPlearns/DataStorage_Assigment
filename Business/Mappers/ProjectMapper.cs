using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Business.Mappers;

public class ProjectMapper(IUserMapper userMapper, 
                           IProductMapper productMapper, 
                           ICustomerMapper customerMapper) 
    : IProjectMapper
{
    private readonly IUserMapper _userMapper = userMapper;
    private readonly ICustomerMapper _customerMapper= customerMapper;

    public ProjectEntity ToEntity(Project model, List<ProductEntity> productEntities, int statusId)
    {
        ProjectEntity entity = new() 
        {
            Title = model.Title,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            UserId = model.AssociatedUser.Id,
            CustomerId = model.AssociatedCustomer.Id,
            StatusId = statusId,
            Products = productEntities
        };

        if (model.Id != default)
            entity.Id = model.Id;
        return entity;
    }

    public ProjectEntity ToEntity(Project model, List<ProductEntity> entities)
    {
        throw new NotImplementedException();
    }

    public Project ToModel(ProjectEntity entity)
    {
        throw new NotImplementedException();
    }

    public Project ToModel(ProjectEntity entity, List<ProductReferenceModel> productReferenceModels)
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
            AssociatedProducts = productReferenceModels
        };
        return project;
    }

    public ProjectReferenceModel? ToReferenceModel(ProjectEntity entity)
    {
        if (entity == null)
        {
            return null;
        }
        return new ProjectReferenceModel
        {
            Id = entity.Id,
            Title = entity.Title,
            Status = entity.Status.StatusName
        };
    }

    public ProjectReferenceModel ToReferenceModel(Project model)
    {
        return new ProjectReferenceModel
        {
            Id = model.Id,
            Title = model.Title,
            Status = model.Status
        };
    }
}
