namespace Business.Mappers;

public interface IBaseMapper<Tentity, TModel, TReferenceModel>
    where Tentity : class
    where TModel : class
    where TReferenceModel : class

{
    Task<Tentity> ToEntity(TModel model);
    Task<Tentity?> ToEntity(TReferenceModel referenceModel);
    TModel ToModel(Tentity entity);
    TReferenceModel ToReferenceModel(Tentity entity);
}
