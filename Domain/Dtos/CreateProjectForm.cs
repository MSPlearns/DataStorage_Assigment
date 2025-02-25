using Domain.Models;

namespace Domain.Dtos;

public class CreateProjectForm
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public User User { get; set; } = null!;

    public Customer Customer { get; set; } = null!;

    public string Status { get; set; } = null!;

    public List<Product> Products { get; set; } = [];
}
