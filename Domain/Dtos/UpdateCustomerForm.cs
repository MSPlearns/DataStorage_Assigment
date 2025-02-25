using Domain.Models;

namespace Domain.Dtos;

public class UpdateCustomerForm
{
    public string CustomerName { get; set; } = null!;
    public List<Project> Projects { get; set; } = [];
}
