using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Presentation.ViewModels;

public partial class CustomerEditViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [ObservableProperty]
    private Customer _currentCustomer = new();

    [ObservableProperty]
    private string _errorMessage= "";

    [ObservableProperty]
    private UpdateCustomerForm _upCustomerForm= new();

    [RelayCommand]
    public void Cancel()
    {
        var customerDetailViewModel = _serviceProvider.GetRequiredService<CustomerDetailViewModel>();
        customerDetailViewModel.CurrentCustomer = CurrentCustomer;
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = customerDetailViewModel;
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        var customerService = _serviceProvider.GetRequiredService<ICustomerService>();
        bool? result = await customerService.UpdateAsync(UpCustomerForm, CurrentCustomer);

        if (result == true)
        {
            var customerDetailViewModel = _serviceProvider.GetRequiredService<CustomerDetailViewModel>();
            customerDetailViewModel.CurrentCustomer = CurrentCustomer;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = customerDetailViewModel;
        }
        else
        {
            ErrorMessage = "Error: Could not update customer.";
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
