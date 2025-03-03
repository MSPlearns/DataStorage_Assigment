using Data.Entities;
using Domain.Dtos;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services
{
    public interface ICustomerService
    {
        Task AddAsync(CreateCustomerForm form);
        Task<bool> AlreadyExists(Expression<Func<CustomerEntity, bool>> predicate);
        Task<bool?> DeleteAsync(int id);
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer?> GetByIdAsync(int id);
        Task UpdateAsync(UpdateCustomerForm form, Customer existingCustomer);
    }
}