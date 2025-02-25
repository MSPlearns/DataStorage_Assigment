namespace Business.Factories;
{
    public interface IBaseFactory<TModel, Tform, Tentity> 
        where TModel : class
        where Tform : class
        where Tentity : class
    {

        TModel FromForm(Tform form);

        TModel FromEntity(Tentity entity);
    }
}
