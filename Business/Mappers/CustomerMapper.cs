using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public class CustomerMapper(IProjectMapper projectMapper) : ICustomerMapper
{
    private readonly IProjectMapper _projectMapper = projectMapper;

    public CustomerEntity ToEntity(Customer model)
    {
        CustomerEntity entity = new()
        {
            CustomerName = model.CustomerName,
            Projects = model.Projects.Select(_projectMapper.ToEntity).ToList()
        };

        if (model.Id != default)
            entity.Id = model.Id;

        return entity;
    }

    public Customer ToModel(CustomerEntity entity)
    {
        return new Customer
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,
            Projects = entity.Projects.Select(_projectMapper.ToModel).ToList()
        };
    }
}
