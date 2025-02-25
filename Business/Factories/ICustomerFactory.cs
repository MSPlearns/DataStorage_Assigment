using Data.Entities;
using Domain.Dtos;
using Domain.Models;

namespace Business.Factories;
public interface ICustomerFactory : IBaseFactory<Customer, CreateCustomerForm, CustomerEntity>
{
}
