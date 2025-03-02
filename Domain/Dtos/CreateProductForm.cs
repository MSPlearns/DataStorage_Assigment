using Domain.Models;

namespace Domain.Dtos;

public class CreateProductForm
{
    public string ProductName { get; set; } = null!;
    public string InputPrice { get; set; }
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];
}
