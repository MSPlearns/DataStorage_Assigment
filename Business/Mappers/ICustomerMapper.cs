using Data.Entities;
using Domain.Models;

namespace Business.Mappers;
public interface ICustomerMapper : IBaseMapper<CustomerEntity, Customer, CustomerReferenceModel>
{

}
