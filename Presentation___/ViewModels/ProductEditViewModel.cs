using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.DependencyInjection;
namespace Presentation.ViewModels;

public partial class ProductEditViewModel : ObservableObject
{
    [ObservableProperty]
    private ObservableObject _currentViewModel = null!;
}
