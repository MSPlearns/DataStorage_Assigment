using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Services;

public interface IUserService : IBaseService<UserEntity, User, CreateUserForm, UpdateUserForm>
{
}
