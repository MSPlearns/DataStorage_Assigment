using Data.Entities;
using Domain.Models;

namespace Business.Mappers;

public interface ICustomerMapper
{
    CustomerEntity ToEntity(Customer model);
    Customer ToModel(CustomerEntity entity, List<ProjectReferenceModel> projectReferences);
    CustomerReferenceModel ToReferenceModel(CustomerEntity entity);
    CustomerReferenceModel ToReferenceModel(Customer model);
}