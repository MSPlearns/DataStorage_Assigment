using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;

public interface IProjectFactory : IBaseFactory<Project, CreateProjectForm, ProjectEntity>
{
}
