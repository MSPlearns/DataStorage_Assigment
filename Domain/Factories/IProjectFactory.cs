using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;

public interface IProjectFactory : IBaseFactory<Project, CreateProjectForm>
{
}
