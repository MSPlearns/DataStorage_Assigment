using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class ProjectReferenceModel
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards

    public string Title { get; set; } = null!;

    public string Status { get; set; } = null!;
}
