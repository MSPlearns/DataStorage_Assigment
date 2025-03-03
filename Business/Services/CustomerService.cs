using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Business.Services;

public class CustomerService(ICustomerRepository customerRepository,
                             ICustomerFactory customerFactory,
                             ICustomerMapper customerMapper,
                             IProjectMapper projectMapper) : ICustomerService

{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly ICustomerFactory _customerFactory = customerFactory;
    private readonly ICustomerMapper _customerMapper = customerMapper;
    private readonly IProjectMapper _projectMapper = projectMapper;

    public async Task AddAsync(CreateCustomerForm form)
    {
        await _customerRepository.BeginTransactionAsync();
        try
        {
            Customer customerModel = _customerFactory.FromForm(form);

            CustomerEntity customerEntity = _customerMapper.ToEntity(customerModel);
            await _customerRepository.AddAsync(customerEntity);
            await _customerRepository.SaveAsync();

            await _customerRepository.CommitTransactionAsync();
        }
        catch(Exception)
        {
            await _customerRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<IEnumerable<Customer>> GetAllAsync()
    {
        List<Customer> customerList = [];
        var result = await _customerRepository.GetAllAsync(query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );
        foreach (var customerEntity in result)
        {
            List<ProjectReferenceModel> associatedProjectsReferences = TransformEntitiesToReferenceModels(customerEntity.Projects);
            customerList.Add(_customerMapper.ToModel(customerEntity, associatedProjectsReferences));
        }
        return customerList;
    }

    public async Task<Customer?> GetByIdAsync(int id)
    {
        CustomerEntity? customerEntity = await _customerRepository.GetAsync(
        x => x.Id == id,
         query => query
        .Include(c => c.Projects)
        .ThenInclude(p => p.Status)
        );

        if (customerEntity == null)
            return null;

        List<ProjectReferenceModel> associatedProjectsReferences = TransformEntitiesToReferenceModels(customerEntity.Projects);
        Customer customer = _customerMapper.ToModel(customerEntity, associatedProjectsReferences);
        return customer;
    }

    public async Task UpdateAsync(UpdateCustomerForm form, Customer existingCustomer)
    {
        await _customerRepository.BeginTransactionAsync();
        try
        {
            existingCustomer.CustomerName = form.CustomerName;

            var updatedEntity = _customerMapper.ToEntity(existingCustomer);
            await _customerRepository.UpdateAsync(x => x.Id == existingCustomer.Id, updatedEntity);
            await _customerRepository.SaveAsync();
            await _customerRepository.CommitTransactionAsync();
        }
        catch (Exception)
        {
            await _customerRepository.RollbackTransactionAsync();
            throw;
        }
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        await _customerRepository.BeginTransactionAsync();
        try
        {
            CustomerEntity? customerEntity = await _customerRepository.GetAsync(x => x.Id == id);
             _customerRepository.Delete(customerEntity!);
            await _customerRepository.SaveAsync();
            await _customerRepository.CommitTransactionAsync();
            return true;
        }
        catch (Exception)
        {
            await _customerRepository.RollbackTransactionAsync();
            throw;
        }
    }

    private List<ProjectReferenceModel> TransformEntitiesToReferenceModels(ICollection<ProjectEntity> projectEntities)
    {
        List<ProjectReferenceModel> projects = [];
        foreach (var project in projectEntities)
        {
            projects.Add(_projectMapper.ToReferenceModel(project));
        }
        return projects;
    }

    public async Task<bool> AlreadyExists(Expression<Func<CustomerEntity, bool>> predicate)
    {
        return await _customerRepository.EntityExistsAsync(predicate);
    }
}
