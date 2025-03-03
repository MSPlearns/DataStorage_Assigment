using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services
{
    public interface IUserService
    {
        Task AddAsync(CreateUserForm form);
        Task<bool> AlreadyExists(Expression<Func<UserEntity, bool>> predicate);
        Task<bool?> DeleteAsync(int id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateUserForm form, User existingUser);
    }
}