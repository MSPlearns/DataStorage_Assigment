
using Domain.Dtos;
using Domain.Models;

namespace Domain.Factories;
public interface ICustomerFactory : IBaseFactory<Customer, CreateCustomerForm, UpdateCustomerForm>
{
}
