using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Services;

public interface IProjectService : IBaseService<ProjectEntity, Project, CreateProjectForm, UpdateProjectForm>
{
}
