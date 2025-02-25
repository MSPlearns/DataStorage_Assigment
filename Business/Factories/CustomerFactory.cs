using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;

public class CustomerFactory(IProjectFactory projectFactory) : ICustomerFactory
{
    private readonly IProjectFactory _projectFactory = projectFactory;
    public Customer FromForm(CreateCustomerForm form)
    {
        return new Customer
        {
            CustomerName = form.CustomerName,
            Projects = form.Projects
        };
    }

    public Customer FromEntity(CustomerEntity entity) {

        return new Customer
        {
            Id = entity.Id,
            CustomerName = entity.CustomerName,
            Projects = entity.Projects.Select(_projectFactory.FromEntity).ToList()
        };

    }
}
