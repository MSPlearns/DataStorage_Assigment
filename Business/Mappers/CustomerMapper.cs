using Business.Services;
using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public class CustomerMapper(IProjectMapper projectMapper, ICustomerService customerService) : ICustomerMapper
{
    private readonly IProjectMapper _projectMapper = projectMapper;
    private readonly ICustomerService _customerService = customerService;

    public async Task<CustomerEntity> ToEntity(Customer model)
    {
        //The use of WhenAll was suggested by AI. It is used to execute multiple tasks concurrently and wait until they are completed before moving on.
        var projects = await Task.WhenAll(model.AssociatedProjects.Select(p => _projectMapper.ToEntity(p)));
        CustomerEntity entity = new()
        {
            CustomerName = model.CustomerName,
            Projects = projects.ToList()
        };

        if (model.Id != default)
            entity.Id = model.Id;

        return entity;
    }

    public async Task<CustomerEntity?> ToEntity(CustomerReferenceModel referenceModel)
    {
        Customer model = await _customerService.GetByIdAsync(referenceModel.Id);
        if (model == null)
        {
            return null;
        }
        CustomerEntity entity = await ToEntity(model);
        return entity;
    }

    public Customer ToModel(CustomerEntity entity)
    {
        return new Customer
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,
            AssociatedProjects = entity.Projects.Select(_projectMapper.ToReferenceModel).ToList()
        };
    }

    public CustomerReferenceModel ToReferenceModel(CustomerEntity entity)
    {
        return new CustomerReferenceModel
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName
        };
    }
}
