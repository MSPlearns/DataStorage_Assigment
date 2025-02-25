namespace Business.Mappers;

public interface IBaseMapper<Tentity, TModel>
    where TModel : class
    where Tentity : class
{
    Tentity ToEntity(TModel model);
    TModel ToModel(Tentity entity);
}
