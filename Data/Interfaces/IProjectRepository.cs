using Data.Entities;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    //TODO: Add custom methods here
    //Note - be more specific about todo:s, i have no idea what custom method i wanted to add here

    //Get all information with the ID
    Task<ProjectEntity?> GetProjectByIdAsync(int projectId);
    Task<bool?> UpdateAsync(ProjectEntity updatedEntity, List<ProductEntity> propjects);
}
