using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class ProductReferenceModel
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards
    public string ProductName { get; set; } = null!;
    public decimal Price { get; set; }

    public bool isSelected { get; set; } = false;
}
