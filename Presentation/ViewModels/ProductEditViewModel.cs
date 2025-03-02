using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
namespace Presentation.ViewModels;

public partial class ProductEditViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;


    [ObservableProperty]
    private Product _currentProduct = new();

    [ObservableProperty]
    private string _errorMessage = "";

    [ObservableProperty]
    private UpdateProductForm _upProductForm = new();

    [RelayCommand]
    public void Cancel()
    {
        if (ErrorMessage == "")
        {
            var productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
            productDetailViewModel.CurrentProduct = CurrentProduct;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = productDetailViewModel;
        }
        GoToProductList();
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        ErrorMessage = "";
        var productService = _serviceProvider.GetRequiredService<IProductService>();
        bool? result = await productService.UpdateAsync(UpProductForm, CurrentProduct);

        if (result == true)
        {
            var productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
            productDetailViewModel.CurrentProduct = CurrentProduct;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = productDetailViewModel;
        }
        else
        {
            ErrorMessage = "Error: Could not create product.";
        }

    }

    [RelayCommand]
    public void GoToProjectList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProjectListViewModel>();
    }

    [RelayCommand]
    public void GoToProductList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
    }

    [RelayCommand]
    public void GoToCustomerList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CustomerListViewModel>();
    }

    [RelayCommand]
    public void GoToUserList()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
    }
}