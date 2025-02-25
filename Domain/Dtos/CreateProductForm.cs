using Domain.Models;

namespace Domain.Dtos;

public class CreateProductForm
{
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public List<Project> Projects { get; set; } = [];
}
