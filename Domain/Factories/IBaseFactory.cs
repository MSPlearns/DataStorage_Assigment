namespace Domain.Factories;

public interface IBaseFactory<TModel, TCreateForm, TUpdateForm>
    where TModel : class
    where TCreateForm : class
    where TUpdateForm : class
{
    TModel FromForm(TCreateForm form);

    TModel FromForm(TUpdateForm form);
}

