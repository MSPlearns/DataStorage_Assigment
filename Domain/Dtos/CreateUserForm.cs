using Domain.Models;

namespace Domain.Dtos;

public class CreateUserForm
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];
}
