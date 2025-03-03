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

public class UserService(IUserRepository userRepository,
                         IUserFactory userFactory,
                         IUserMapper userMapper, 
                         IProjectMapper projectMapper) : IUserService
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IUserFactory _userFactory = userFactory;
    private readonly IUserMapper _userMapper = userMapper;
    private readonly IProjectMapper _projectMapper = projectMapper;
    public async Task AddAsync(CreateUserForm form)
    {
        await _userRepository.BeginTransactionAsync();
        try
        {
            User userModel = _userFactory.FromForm(form);
            UserEntity userEntity = _userMapper.ToEntity(userModel);
            await _userRepository.AddAsync(userEntity);
            await _userRepository.SaveAsync();
            await _userRepository.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _userRepository.RollbackTransactionAsync();
            throw;
        }
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
        UserEntity? userEntity = await _userRepository.GetAsync(
        x => x.Id == id,
         query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );

        if (userEntity == null)
            return null;

        List<ProjectReferenceModel> associatedProjectsReferences = TransformEntitiesToReferenceModels(userEntity.Projects);
        User user = _userMapper.ToModel(userEntity, associatedProjectsReferences);
        return user;
    }

    public async Task UpdateAsync(UpdateUserForm form, User existingUser)
    {
        await _userRepository.BeginTransactionAsync();
        try
        {
            existingUser.FirstName = form.FirstName;
            existingUser.LastName = form.LastName;
            existingUser.Email = form.Email;
            var updatedEntity = _userMapper.ToEntity(existingUser);
            await _userRepository.UpdateAsync(x => x.Id == existingUser.Id, updatedEntity);
            await _userRepository.SaveAsync();
            await _userRepository.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _userRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        await _userRepository.BeginTransactionAsync();
        try
        {
            UserEntity? userEntity = await _userRepository.GetAsync(x => x.Id == id);
            _userRepository.Delete(userEntity!);
            await _userRepository.SaveAsync();
            await _userRepository.CommitTransactionAsync();
            return true;
        }
        catch (Exception)
        {
            await _userRepository.RollbackTransactionAsync();
            throw;
        }
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
