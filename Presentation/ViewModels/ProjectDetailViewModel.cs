using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;


namespace Presentation.ViewModels;

public partial class ProjectDetailViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    public ProjectDetailViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    [ObservableProperty]
    private Project _currentProject = new();


    [RelayCommand]
    public async Task DeleteProject()
    {
        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        bool? result = await projectService.DeleteAsync(CurrentProject.Id);
        if (result == true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProjectListViewModel>();
        }
    }

    [RelayCommand]
    public async Task GoToProjectEdit()
    {
        var projectEditViewModel = _serviceProvider.GetRequiredService<ProjectEditViewModel>();
        projectEditViewModel.CurrentProject = CurrentProject;
        projectEditViewModel._originalProject = CurrentProject;

        projectEditViewModel.UpProjectForm = new UpdateProjectForm
        {
            Title = CurrentProject.Title,
            Description = CurrentProject.Description,
            AssociatedProducts = CurrentProject.AssociatedProducts,
            StartDate = CurrentProject.StartDate,
            EndDate = CurrentProject.EndDate,
            AssociatedUser = CurrentProject.AssociatedUser,
            AssociatedCustomer = CurrentProject.AssociatedCustomer,
            Status = CurrentProject.Status
        };

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();

        projectEditViewModel.UpdateIsSelectedInAvailableProductsAsync();
        mainViewModel.CurrentViewModel = projectEditViewModel;
    }

    #region navigationMethods
    [RelayCommand]
    public async Task GoToProductDetail(ProductReferenceModel selectedProductReference)
    {
        var productService = _serviceProvider.GetRequiredService<IProductService>();
        var selectedProductModel = await productService.GetByIdAsync(selectedProductReference.Id);

        var productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
        productDetailViewModel.CurrentProduct = selectedProductModel;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = productDetailViewModel;
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
