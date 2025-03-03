using Data.Entities;
using Domain.Models;

namespace Business.Mappers
{
    public interface IProductMapper
    {
        ProductEntity ToEntity(Product model);
        Product ToModel(ProductEntity entity, List<ProjectReferenceModel> projectReferenceModels);
        ProductReferenceModel ToReferenceModel(Product model);
        ProductReferenceModel ToReferenceModel(ProductEntity entity);
    }
}