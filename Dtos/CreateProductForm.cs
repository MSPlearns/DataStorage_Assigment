namespace Dtos;

public class CreateProductForm
{
    public int Id { get; set; }
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }
    public List<Project> Projects { get; set; } = [];
}
