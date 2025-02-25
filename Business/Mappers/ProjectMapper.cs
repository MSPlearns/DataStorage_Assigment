using Business.Services;
using Data.Entities;
using Data.Interfaces;
using Domain.Models;

namespace Business.Mappers;

public class ProjectMapper(IUserMapper userMapper, IProductMapper productMapper,ICustomerMapper customerMapper, IStatusTypeRepository statusTypeRepository) : IProjectMapper
{
    private readonly IUserMapper _userMapper = userMapper;
    private readonly IProductMapper _productMapper = productMapper;
    private readonly ICustomerMapper _customerMapper= customerMapper;
    private readonly IStatusTypeRepository _statusTypeRepository = statusTypeRepository;
    public ProjectEntity ToEntity(Project model)
    {
       ProjectEntity entity = new() 
        {
            Title = model.Title,
            Description = model.Description,
            StartDate = model.StartDate,
            EndDate = model.EndDate,
            UserId = model.AssociatedUser.Id,
            CustomerId = model.AssociatedCustomer.Id,
            StatusId = _statusTypeRepository.GetAsync(x => x.StatusName == model.Status).Id,
            Products = model.Products.Select(_productMapper.ToEntity).ToList()
       };

        if (model.Id != default)
            entity.Id = model.Id;
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
            AssociatedUser = _userMapper.ToModel(entity.User),
            AssociatedCustomer = _customerMapper.ToModel(entity.Customer),
            Status = entity.Status.StatusName,
            Products = entity.Products.Select(_productMapper.ToModel).ToList()
        };
        return project;
    }
}
