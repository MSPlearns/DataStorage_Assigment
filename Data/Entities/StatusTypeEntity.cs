using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
[Index(nameof(StatusName), IsUnique = true)]
public class StatusTypeEntity
{
    [Key]
    public int Id { get; set; }

    [Column(TypeName = "nvarchar(50)")]
    public string StatusName { get; set; } = null!;
    public ICollection<ProjectEntity> Projects { get; set; } = [];
    //One-to-many, one status can have many projects
}