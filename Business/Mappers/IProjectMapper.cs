using Data.Entities;
using Domain.Models;

namespace Business.Mappers
{
    public interface IProjectMapper
    {
        ProjectEntity ToEntity(Project model, List<ProductEntity> productEntities, int statusId);
        Project ToModel(ProjectEntity entity, List<ProductReferenceModel> productReferenceModels);
        ProjectReferenceModel ToReferenceModel(Project model);
        ProjectReferenceModel ToReferenceModel(ProjectEntity entity);
    }
}