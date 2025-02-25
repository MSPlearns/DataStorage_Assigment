using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Project
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public User AssociatedUser { get; set; } = null!;

    public Customer AssociatedCustomer { get; set; } = null!;

    public string Status { get; set; } = null!;

    public List<Product> Products { get; set; } = [];


}
