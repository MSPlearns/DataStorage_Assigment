using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class User
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards

    [Required(ErrorMessage = "*First name field is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "*First name must contain 2 to 50 characters")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜäöüßøØåÅæÆ'’\s-]+$", ErrorMessage = "*First name cannot contain special characters")]
    public string FirstName { get; set; } = null!;

    [Required(ErrorMessage = "*Last name field is required")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "*Last name must contain 2 to 50 characters")]
    [RegularExpression(@"^[a-zA-ZáéíóúÁÉÍÓÚñÑüÜäöüßøØåÅæÆ'’\s-]+$", ErrorMessage = "*Last name cannot contain special characters")]
    public string LastName { get; set; } = null!;

    [Required(ErrorMessage = "*Email is required")]
    [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,6}$", ErrorMessage = "*Email must be a valid email address")]
    public string Email { get; set; } = null!;
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];
}
