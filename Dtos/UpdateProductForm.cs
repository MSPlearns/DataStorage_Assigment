namespace Dtos;

public class UpdateProductForm
{
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public List<Project> Projects { get; set; } = [];
}
