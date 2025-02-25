using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class User
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<Project> Projects { get; set; } = [];
}
