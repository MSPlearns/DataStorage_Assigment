using Domain.Models;

namespace Domain.Dtos;

public class UpdateProductForm
{
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];
}
