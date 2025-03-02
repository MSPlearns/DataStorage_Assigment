using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.Services;

public class UserService(IUserRepository userRepository, IUserFactory userFactory, IUserMapper userMapper, IProjectRepository projectRepository, IProjectMapper projectMapper) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserFactory _userFactory = userFactory;
    private readonly IUserMapper _userMapper = userMapper;
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly IProjectMapper _projectMapper = projectMapper;
    public async Task<bool?> AddAsync(CreateUserForm form)
    {
        User userModel = _userFactory.FromForm(form);

        List<ProjectEntity> projects = await GetAssociatedEntitiesAsync(userModel);

        UserEntity userEntity = _userMapper.ToEntity(userModel, projects);
        return await _userRepository.AddAsync(userEntity);
    }


    public async Task<IEnumerable<User>> GetAllAsync()
    { 
        List<User> userList = [];
        var result = await _userRepository.GetAllAsync(query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );


        foreach (var userEntity in result)
        {
            List<ProjectReferenceModel> associatedProjectsReferences = TransformEntitiesToReferenceModels(userEntity.Projects);
            userList.Add(_userMapper.ToModel(userEntity, associatedProjectsReferences));
        }
        return userList;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        var result = await _userRepository.GetAsync(
        x => x.Id == id,
         query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );
        if (result == null)
        {
            return null;
        }
        List<ProjectReferenceModel> associatedProjectsReferences = TransformEntitiesToReferenceModels(result.Projects);
        User user = _userMapper.ToModel(result, associatedProjectsReferences);
        return user;
    }

    public async Task<bool?> UpdateAsync(UpdateUserForm form, User existingUser)
    {
        existingUser.FirstName = form.FirstName;
        existingUser.LastName = form.LastName;
        existingUser.Email = form.Email;
        List<ProjectEntity> projects = await GetAssociatedEntitiesAsync(existingUser);
        var updatedEntity =  _userMapper.ToEntity(existingUser, projects);

        return await _userRepository.UpdateAsync(x => x.Id == existingUser.Id, updatedEntity);
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        return await _userRepository.DeleteAsync(x => x.Id == id);
    }

    private async Task<List<ProjectEntity>> GetAssociatedEntitiesAsync(User userModel)
    {
        List<ProjectEntity> projects = [];
        foreach (var project in userModel.AssociatedProjects)
        {
            ProjectEntity projectEntity = await _projectRepository.GetProjectByIdAsync(project.Id);
            if (projectEntity != null)
            {
                projects.Add(projectEntity);
            }
        }

        return projects;
    }

    private List<ProjectReferenceModel> TransformEntitiesToReferenceModels(ICollection<ProjectEntity> projectEntities)
    {
        List<ProjectReferenceModel> projects = [];
        foreach (var project in projectEntities)
        {
            projects.Add(_projectMapper.ToReferenceModel(project));
        }

        return projects;
    }

    public async Task<bool> AlreadyExists(Expression<Func<UserEntity, bool>> predicate)
    {
        return await _userRepository.EntityExistsAsync(predicate);
    }
}
