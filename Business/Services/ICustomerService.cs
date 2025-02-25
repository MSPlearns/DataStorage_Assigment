using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Services;

public interface ICustomerService : IBaseService<CustomerEntity, Customer, CreateCustomerForm, UpdateCustomerForm>
{
}
