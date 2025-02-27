using Domain.Models;

namespace Domain.Dtos;

public class UpdateCustomerForm
{
    public string CustomerName { get; set; } = null!;
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];
}
