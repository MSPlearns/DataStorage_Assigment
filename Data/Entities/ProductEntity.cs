
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Index(nameof(ProductName), IsUnique = true)]
public class ProductEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(150)")]
    public string ProductName { get; set; } = null!;

    public decimal Price { get; set; }
    public ICollection<ProjectEntity> Projects { get; set; } = [];
}

