using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

[Index(nameof(CustomerName), IsUnique = true)]
public class CustomerEntity
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(50)")]
    public string CustomerName { get; set; } = null!;

    public ICollection<ProjectEntity> Projects { get; set; } = [];
}
