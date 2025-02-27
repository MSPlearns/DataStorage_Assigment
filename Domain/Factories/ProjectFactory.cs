using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;
public  class ProjectFactory : IProjectFactory
{
     public Project FromForm(CreateProjectForm form)
    {
        return new Project()
        {
            Title = form.Title,
            Description = form.Description,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            AssociatedUser = form.AssociatedUser,
            AssociatedCustomer = form.AssociatedCustomer,
            Status = form.Status,
            AssociatedProducts = form.AssociatedProducts
        };
    }

    public Project FromForm(UpdateProjectForm form)
    {
        return new Project()
        {
            Title = form.Title,
            Description = form.Description,
            StartDate = form.StartDate,
            EndDate = form.EndDate,
            AssociatedUser = form.AssociatedUser,
            AssociatedCustomer = form.AssociatedCustomer,
            Status = form.Status,
            AssociatedProducts = form.AssociatedProducts
        };
    }
}
