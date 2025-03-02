using Domain.Models;

namespace Domain.Dtos;

public class UpdateProductForm
{
    public string ProductName { get; set; } = null!;
    public string InputPrice { get; set; }
}
