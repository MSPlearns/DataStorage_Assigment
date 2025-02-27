using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class UserReferenceModel
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards
    public string FullName { get; set; } = null!;
}
