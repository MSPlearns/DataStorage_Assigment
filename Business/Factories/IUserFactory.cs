using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;

public interface IUserFactory : IBaseFactory<User, CreateUserForm>
{
}
