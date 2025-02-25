using Data.Interfaces;
using Domain.Dtos;
using Business.Factories;
using Domain.Models;

namespace Business.Services;

public class CustomerService(ICustomerRepository customerRepository, ICustomerFactory customerFactory) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerFactory _customerFactory = customerFactory;

    public async Task<bool> AddAsync(CreateCustomerForm form)
    {
        Customer customer = _customerFactory.FromForm(form);
        ///TODO: Add a mapper class in Business.Helpers to map Customer to CustomerEntity and vice versa. 
        ///TODO: Move the ToModel logic from the factory to the mapper class.
        ///TODO: Move factories back to domain layer.
        ///TODO: Use the mapper to map the customer to customer entity.

        //_customerRepository.AddAsync(customerEntity);
        return true;

    }

    public async Task<bool?> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        List<Customer> customerList = new();
        var result = await _customerRepository.GetAllAsync();
        foreach (var item in result)
        {
            Customer customer = _customerFactory.FromEntity(item);
            customerList.Add(customer);
        }
        return customerList;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<Customer?> UpdateAsync(int id, UpdateCustomerForm form)
    {
        throw new NotImplementedException();
    }
}
