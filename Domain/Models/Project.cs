using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Project
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards

    [Required(ErrorMessage = "*Title field is required")]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    public DateTime StartDate { get; set; } = DateTime.Now;

    public DateTime? EndDate { get; set; }

    [Required(ErrorMessage = "*User field is required")]
    public UserReferenceModel AssociatedUser { get; set; } = null!;
    
    [Required(ErrorMessage = "*User field is required")]
    public CustomerReferenceModel AssociatedCustomer { get; set; } = null!;

    [Required(ErrorMessage = "*Status field is required")]
    public string Status { get; set; } = null!;

    public List<ProductReferenceModel> AssociatedProducts { get; set; } = [];


}
