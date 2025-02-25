using Domain.Models;

namespace Domain.Dtos;

public class UpdateCustomerForm
{
    public int Id { get; private set; }
    public string CustomerName { get; set; } = null!;
    public List<Project> Projects { get; set; } = [];
}
