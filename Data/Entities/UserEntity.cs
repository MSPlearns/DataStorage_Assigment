using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
[Index(nameof(Email), IsUnique = true)]
public class UserEntity
{
    [Key]
    public int Id { get; set; }
    [Column(TypeName ="nvarchar(50)")]
    public string FirstName { get; set; } = null!;
    [Column(TypeName = "nvarchar(50)")]
    public string LastName { get; set; } = null!;
    [Column(TypeName = "nvarchar(150)")]
    public string Email { get; set; } = null!;

    public ICollection<ProjectEntity> Projects { get; set; } = [];
    //One-to-many, one user can have many projects
}

