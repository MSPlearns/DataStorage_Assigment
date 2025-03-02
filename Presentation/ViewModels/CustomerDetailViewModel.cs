using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Presentation.ViewModels;

public partial class CustomerDetailViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [ObservableProperty]
    private Customer _currentCustomer = new();


    [RelayCommand]

    public async Task DeleteCustomer()
    {
        var customerService = _serviceProvider.GetRequiredService<ICustomerService>();
        bool? result = await customerService.DeleteAsync(CurrentCustomer.Id);
        if (result == true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CustomerListViewModel>();
        }

    }

    [RelayCommand]
    public void GoToCustomerEdit()
    {
        var customerEditViewModel = _serviceProvider.GetRequiredService<CustomerEditViewModel>();
        customerEditViewModel.CurrentCustomer = CurrentCustomer;
        customerEditViewModel._originalCustomer = CurrentCustomer;
        customerEditViewModel.UpCustomerForm.CustomerName = CurrentCustomer.CustomerName;
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = customerEditViewModel;
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

    #region navigationMethods
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

