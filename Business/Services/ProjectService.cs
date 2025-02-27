using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;
namespace Business.Services;

public class ProjectService(IProjectRepository projectRepository, IProjectFactory projectFactory, IProjectMapper projectMapper) : IProjectService
{
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProjectFactory _projectFactory = projectFactory;
    private readonly IProjectMapper _projectMapper = projectMapper;

    public async Task<bool?> AddAsync(CreateProjectForm form)
    {
        Project projectModel = _projectFactory.FromForm(form);
        ProjectEntity projectEntity = await _projectMapper.ToEntity(projectModel);
        return await _projectRepository.AddAsync(projectEntity);
    }

    public async Task<IEnumerable<Project>> GetAllAsync()
    {
        var result = await _projectRepository.GetAllAsync();
        return result.Select(x => _projectMapper.ToModel(x));
    }

    public async Task<Project?> GetByIdAsync(int id)
    {
        var result = await _projectRepository.GetAsync(x => x.Id == id);
        if (result == null)
        {
            return null;
        }

        Project project = _projectMapper.ToModel(result);
        return project;
    }

    public async Task<Project?> GetByTitleAsync(string title)
    {
        var result = await _projectRepository.GetAsync(x => x.Title == title);
        if (result == null)
        {
            return null;
        }

        Project project = _projectMapper.ToModel(result);
        return project;
    }

    public async Task<bool?> UpdateAsync(UpdateProjectForm form, Project existingProject)
    {
        existingProject.Title = form.Title;
        existingProject.Description = form.Description;
        existingProject.StartDate = form.StartDate;
        existingProject.EndDate = form.EndDate;
        existingProject.AssociatedUser = form.AssociatedUser;
        existingProject.AssociatedCustomer = form.AssociatedCustomer;
        existingProject.AssociatedProducts = form.AssociatedProducts;
        var updatedEntity = await _projectMapper.ToEntity(existingProject);

        return await _projectRepository.UpdateAsync(x => x.Id == existingProject.Id, updatedEntity);
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        return await _projectRepository.DeleteAsync(x => x.Id == id);
    }
}
