using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models;

public class Product
{
    public int Id { get; set; } = default!; // EF Core will assign an id. It gets mapped back to the model afterwards

    [Required(ErrorMessage = "*Product name is required")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "*Product name must contain 2 to 150 characters")]
    [RegularExpression(@"^[\p{L}\p{N}\s\-\.&'()]+$", ErrorMessage = "*Product name can only contain letters, numbers, spaces, hyphens, periods, ampersands, apostrophes, and parentheses.")]
    public string ProductName { get; set; } = null!;


    //WPF Text box does not support decimal type, so I had to use a string to store the price in the form.
    //The form and the model have inputPrice to allow for validation,
    //the factory parses the value from the form.inputPrice and assigns it to Price (decimal) before sending it to the database

    [Required(ErrorMessage = "*Price is required")]
    [Range(0.01, double.MaxValue, ErrorMessage = "*Price must be a positive number greater than 0.")]
    [RegularExpression(@"^\d+(,\d{1,2})?$", ErrorMessage = "*Price must be a valid decimal number with up to two decimal places separated by a comma.")]
    public string InputPrice { get; set; }

    //This is the actual price that will be stored in the database
    public decimal Price { get; set; }
    public List<ProjectReferenceModel> AssociatedProjects { get; set; } = [];
}
