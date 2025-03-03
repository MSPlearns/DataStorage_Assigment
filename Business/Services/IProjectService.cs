using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services
{
    public interface IProjectService
    {
        Task AddAsync(CreateProjectForm form);
        Task<bool> AlreadyExists(Expression<Func<ProjectEntity, bool>> predicate);
        Task<bool?> DeleteAsync(int id);
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateProjectForm form, Project existingProject);
    }
}