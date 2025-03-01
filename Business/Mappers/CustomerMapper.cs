using Business.Services;
using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public class CustomerMapper : ICustomerMapper
{

    public CustomerEntity ToEntity(Customer model, List<ProjectEntity> projectEntities)
    {
        CustomerEntity entity = new()
        {
            CustomerName = model.CustomerName,
            Projects = projectEntities
        };

        if (model.Id != default)
            entity.Id = model.Id;

        return entity;
    }

    public Customer ToModel(CustomerEntity entity, List<ProjectReferenceModel> projectReferences)
    {
        return new Customer
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,
            AssociatedProjects = projectReferences
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

    public CustomerReferenceModel ToReferenceModel(Customer model)
    {
        return new CustomerReferenceModel
        {
            Id = model.Id,
            CustomerName = model.CustomerName
        };
    }


}
