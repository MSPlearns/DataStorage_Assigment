namespace Business.Mappers;

public interface IBaseMapper<Tentity, TModel, TReferenceModel, TSecondaryEntity, TSecondaryReference>
    where Tentity : class
    where TModel : class
    where TSecondaryEntity : class
    where TReferenceModel : class
    where TSecondaryReference : class

{
    Tentity ToEntity(TModel model, List<TSecondaryEntity> entities);
    TModel ToModel(Tentity entity, List<TSecondaryReference> secondaryReferences);
    TReferenceModel ToReferenceModel(Tentity entity);
}
