using Data.Entities;

namespace Data.Interfaces;

public interface IProjectRepository : IBaseRepository<ProjectEntity>
{
    Task<bool?> UpdateAsync(ProjectEntity updatedEntity, List<ProductEntity> projects);
}
