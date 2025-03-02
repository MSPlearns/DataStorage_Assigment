using Domain.Models;

namespace Domain.Dtos;

public class CreateProjectForm
{
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime? EndDate { get; set; }

    public UserReferenceModel AssociatedUser { get; set; } = null!;

    public CustomerReferenceModel AssociatedCustomer { get; set; } = null!;

    public string Status { get; set; } = null!;

    public List<ProductReferenceModel> AssociatedProducts { get; set; } = [];
}
