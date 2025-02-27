namespace Domain.Models;

public class Customer
{
    public int Id { get; set; } = default; // EF Core will assign an id. It gets mapped back to the model afterwards
    public string CustomerName { get; set; } = null!;
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];

    
   
}
