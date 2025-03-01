using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public interface IProjectMapper : IBaseMapper<ProjectEntity, Project, ProjectReferenceModel, ProductEntity, ProductReferenceModel>
{
    ProjectEntity ToEntity(Project model, List<ProductEntity> productEntities, int statusId);
}
