using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;

public interface IUserFactory : IBaseFactory<User, CreateUserForm, UpdateUserForm>
{
}
