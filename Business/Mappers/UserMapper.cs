using Business.Services;
using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public class UserMapper : IUserMapper
{
    public  UserEntity ToEntity(User model, List<ProjectEntity> projectEntities)
    {
        UserEntity entity = new()
        {
            Id = model.Id,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Email = model.Email,
            Projects = projectEntities
        };
        if (model.Id != default)
            entity.Id = model.Id;

        return entity;
    }


    public User ToModel(UserEntity entity, List<ProjectReferenceModel> projectReferences)
    {
        return new User 
        {
            Id = entity.Id,
            FirstName = entity.FirstName,
            LastName = entity.LastName,
            Email = entity.Email,
            AssociatedProjects = projectReferences
        };
    }

    public UserReferenceModel ToReferenceModel(UserEntity entity)
    {
        return new UserReferenceModel
        {
            Id = entity.Id,
            FullName = $"{entity.FirstName} {entity.LastName}",
            Email = entity.Email,
        };
    }

    public UserReferenceModel ToReferenceModel(User model)
    {
        return new UserReferenceModel
        {
            Id = model.Id,
            FullName = $"{model.FirstName} {model.LastName}",
            Email = model.Email,
        };
    }
}
