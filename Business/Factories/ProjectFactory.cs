using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;
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

     public Project FromEntity(ProjectEntity entity)
    {
        return new Project()
        {
            Id = entity.Id,
            Title = entity.Title,
            Description = entity.Description,
            StartDate = entity.StartDate,
            EndDate = entity.EndDate
        };
    }
}
