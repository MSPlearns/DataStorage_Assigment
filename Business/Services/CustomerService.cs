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
                             IProjectRepository projectRepository,
                             IProjectMapper projectMapper,
                             IStatusTypeRepository statusTypeRepository) : ICustomerService
                             
{
    private readonly ICustomerRepository _customerRepository = customerRepository;
    private readonly IProjectRepository _projectRepository = projectRepository;
    private readonly ICustomerFactory _customerFactory = customerFactory;
    private readonly ICustomerMapper _customerMapper = customerMapper;
    private readonly IProjectMapper _projectMapper = projectMapper;

    public async Task<bool?> AddAsync(CreateCustomerForm form)
    {
        Customer customerModel = _customerFactory.FromForm(form);

        List<ProjectEntity> projects = await GetAssociatedEntitiesAsync(customerModel);
        CustomerEntity customerEntity = _customerMapper.ToEntity(customerModel, projects);
        return await _customerRepository.AddAsync(customerEntity);
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
        {
            return null;
        }
        List<ProjectReferenceModel> associatedProjectsReferences = TransformEntitiesToReferenceModels(customerEntity.Projects);
        Customer customer = _customerMapper.ToModel(customerEntity, associatedProjectsReferences);
        return customer;
    }

    public async Task<bool?> UpdateAsync(UpdateCustomerForm form, Customer existingCustomer)
    {
        existingCustomer.CustomerName = form.CustomerName;

       List<ProjectEntity> projects = await GetAssociatedEntitiesAsync(existingCustomer);

        var updatedEntity = _customerMapper.ToEntity(existingCustomer, projects);

        return await _customerRepository.UpdateAsync(x => x.Id == existingCustomer.Id, updatedEntity);
    }

    public async Task<bool?> DeleteAsync(int id)
    {
        return await _customerRepository.DeleteAsync(x => x.Id == id);
    }

    private async Task<List<ProjectEntity>> GetAssociatedEntitiesAsync(Customer customerModel)
    {
        List<ProjectEntity> projects = [];
        foreach (var project in customerModel.AssociatedProjects)
        {
            ProjectEntity projectEntity = await _projectRepository.GetProjectByIdAsync(project.Id);
            if (projectEntity != null)
            {
                projects.Add(projectEntity);
            }
        }

        return projects;
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

}
