using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Project
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards

    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime EndDate { get; set; }

    public UserReferenceModel AssociatedUser { get; set; } = null!;

    public CustomerReferenceModel AssociatedCustomer { get; set; } = null!;

    public string Status { get; set; } = null!;

    public List<ProductReferenceModel> AssociatedProducts { get; set; } = [];


}
