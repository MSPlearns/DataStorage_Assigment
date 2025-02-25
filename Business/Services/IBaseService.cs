namespace Business.Services;

public interface IBaseService<Tentity, TModel, TCreateForm, TUpdateForm> 
    where Tentity : class 
    where TModel : class
    where TCreateForm : class
    where TUpdateForm : class
{
    Task<bool> AddAsync(TCreateForm form);
    Task<TModel?> UpdateAsync(int id, TUpdateForm form);
    Task<IEnumerable<TModel>> GetAllAsync();
    Task<TModel?> GetByIdAsync(int id);
    Task<bool?> DeleteAsync(int id);

}
