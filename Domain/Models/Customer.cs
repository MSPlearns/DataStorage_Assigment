using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Customer
{
    public int Id { get; set; } = default; // EF Core will assign an id. It gets mapped back to the model afterwards

    [Required(ErrorMessage = "*First name field is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "*First name must contain 2 to 50 characters")]
    [RegularExpression(@"^[\p{L}\p{N}\s\-\.&]+$", ErrorMessage = "*Customer name can only contain letters, numbers, spaces, hyphens, periods, and &.")]
    public string CustomerName { get; set; } = null!;
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];

    
   
}
