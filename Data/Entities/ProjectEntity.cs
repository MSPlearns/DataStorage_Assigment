using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
[Index(nameof(Title), IsUnique = true)]
public class ProjectEntity
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName = "nvarchar(150)")]
    public string Title { get; set; } = null!;
    public string? Description { get; set; }

    [Column(TypeName = "Date")]
    public DateTime StartDate { get; set; } = DateTime.Now;

    [Column(TypeName = "Date")]
    public DateTime EndDate { get; set; }

    public int UserId { get; set; }
    public UserEntity User { get; set; } = null!;

    public int CustomerId { get; set; }
    public CustomerEntity Customer { get; set; } = null!;

    public int StatusId { get; set; }
    public StatusTypeEntity Status { get; set; } = null!;

    public ICollection<ProductEntity> Products { get; set; } = [];
    //One-to-many, one project can have many products

}

