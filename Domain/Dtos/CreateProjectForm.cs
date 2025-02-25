using Domain.Models;

namespace Domain.Dtos;

public class CreateProjectForm
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public User AssociatedUser { get; set; } = null!;

    public Customer AssociatedCustomer { get; set; } = null!;

    public string Status { get; set; } = null!;

    public List<Product> Products { get; set; } = [];
}
