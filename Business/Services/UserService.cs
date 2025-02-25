using Domain.Dtos;

namespace Business.Services;

public class UserService : IUserService
{
    public Task<bool?> AddAsync(CreateUserForm form)
    {
        throw new NotImplementedException();
    }

    public Task<bool?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<UserModel>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<UserModel?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel?> UpdateAsync(int id, UpdateUserForm form)
    {
        throw new NotImplementedException();
    }
}
