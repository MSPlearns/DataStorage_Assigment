using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public interface IUserMapper
{
    UserEntity ToEntity(User model);
    User ToModel(UserEntity entity, List<ProjectReferenceModel> projectReferences);
    UserReferenceModel ToReferenceModel(UserEntity entity);
    UserReferenceModel ToReferenceModel(User model);
}