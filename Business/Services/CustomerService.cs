using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;
using System.Linq.Expressions;

namespace Business.Services;

public class CustomerService(ICustomerRepository customerRepository, ICustomerFactory customerFactory, ICustomerMapper customerMapper) : ICustomerService
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerFactory _customerFactory = customerFactory;
    private readonly ICustomerMapper _customerMapper = customerMapper;

    public async Task<bool?> AddAsync(CreateCustomerForm form)
    {
        Customer customerModel = _customerFactory.FromForm(form);
        CustomerEntity customerEntity= _customerMapper.ToEntity(customerModel);
        return await _customerRepository.AddAsync(customerEntity); 
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        var result = await _customerRepository.GetAllAsync();
        return result.Select(x => _customerMapper.ToModel(x));
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        var result = await _customerRepository.GetAsync(x => x.Id == id);
        if (result == null)
        {
            return null;
        }

        Customer customer = _customerMapper.ToModel(result);
        return customer;
    }

    public async Task<Customer?> GetByName(string name)
    {
        var result = await _customerRepository.GetAsync(x => x.CustomerName == name);
        if (result == null)
        {
            return null;
        }

        Customer customer = _customerMapper.ToModel(result);
        return customer;
    }


    public async Task<bool?> UpdateAsync(UpdateCustomerForm form, Customer existingCustomer)
    {
        existingCustomer.CustomerName = form.CustomerName;
        existingCustomer.Projects = form.Projects;
        var updatedEntity = _customerMapper.ToEntity(existingCustomer);

        return await _customerRepository.UpdateAsync(x => x.Id == existingCustomer.Id, updatedEntity);
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        return await _customerRepository.DeleteAsync(x => x.Id == id);
    }
}
