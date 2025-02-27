using Business.Services;
using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

internal class UserMapper(IProjectMapper projectMapper, IUserService userService) : IUserMapper
{
    private readonly IProjectMapper _projectMapper = projectMapper;
    private readonly IUserService _userService = userService;
    public async Task<UserEntity> ToEntity(User model)
    {
        //The use of WhenAll was suggested by AI. It is used to execute multiple tasks concurrently and wait until they are completed before moving on.
        var projects = await Task.WhenAll(model.AssociatedProjects.Select(p => _projectMapper.ToEntity(p)));

        UserEntity entity = new()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Projects = projects.ToList()
        };
        if (model.Id != default)
            entity.Id = model.Id;

        return entity;
    }

    public async Task<UserEntity?> ToEntity(UserReferenceModel referenceModel)
    {
        User model = await _userService.GetByIdAsync(referenceModel.Id);
        if (model == null)
        {
            return null;
        }
        UserEntity entity = await ToEntity(model);
        return entity;
    }

    public User ToModel(UserEntity entity)
    {
        return new User 
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            AssociatedProjects = entity.Projects.Select(_projectMapper.ToReferenceModel).ToList()
        };
    }

    public UserReferenceModel ToReferenceModel(UserEntity entity)
    {
        return new UserReferenceModel
        {
            Id = entity.Id,
            FullName = $"{entity.FirstName} {entity.LastName}",
        };
    }
}
