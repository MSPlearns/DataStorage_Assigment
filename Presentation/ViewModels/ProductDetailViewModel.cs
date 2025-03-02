using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.ViewModels;

public partial class ProductDetailViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    [ObservableProperty]
    private Product _currentProduct = new();

    [RelayCommand]
    public async Task DeleteProduct()
    {
        var productService = _serviceProvider.GetRequiredService<IProductService>();
        bool? result = await productService.DeleteAsync(CurrentProduct.Id);
        if (result == true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductListViewModel>();
        }
    }

    #region navigationMethods
    [RelayCommand]
    public void GoToProductEdit()
    {
        var productEditViewModel = _serviceProvider.GetRequiredService<ProductEditViewModel>();
        productEditViewModel.CurrentProduct = CurrentProduct;
        productEditViewModel._originalProduct = CurrentProduct;

        productEditViewModel.UpProductForm.ProductName = CurrentProduct.ProductName;
        productEditViewModel.UpProductForm.InputPrice = CurrentProduct.Price.ToString();
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = productEditViewModel;
    }

    [RelayCommand]
    public async Task GoToProjectDetail(ProjectReferenceModel selectedProjectReference)
    {
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        var selectedProjectModel = await projectService.GetByIdAsync(selectedProjectReference.Id);

        var projectDetailViewModel = _serviceProvider.GetRequiredService<ProjectDetailViewModel>();
        projectDetailViewModel.CurrentProject = selectedProjectModel;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = projectDetailViewModel;
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
    #endregion navigationMethods
}
