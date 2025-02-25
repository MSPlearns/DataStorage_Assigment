using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

internal class UserMapper(IProjectMapper projectMapper) : IUserMapper
{
    private readonly IProjectMapper _projectMapper = projectMapper;
    public UserEntity ToEntity(User model)
    {
        UserEntity entity = new()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Projects = model.Projects.Select(_projectMapper.ToEntity).ToList()
        };
        if (model.Id != default)
            entity.Id = model.Id;

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
            Projects = entity.Projects.Select(_projectMapper.ToModel).ToList()
        };
    }
}
