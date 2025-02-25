using Data.Entities;
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
            EndDate = form.EndDate
        };
    }
}
