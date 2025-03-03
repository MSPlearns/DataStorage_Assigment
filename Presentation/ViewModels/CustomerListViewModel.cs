using Business.Mappers;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;

namespace Presentation.ViewModels;

public partial class CustomerListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ICustomerService _customerService;
    private readonly ICustomerMapper _customerMapper;


    [ObservableProperty]
    private ObservableCollection<CustomerReferenceModel> _customers = [];

    [ObservableProperty]
    private Customer _selectedCustomer = null!;

    public CustomerListViewModel(IServiceProvider serviceProvider, ICustomerService customerService, ICustomerMapper customerMapper)
    {
        _serviceProvider = serviceProvider;
        _customerService = customerService;
        _customerMapper = customerMapper;
        LoadCustomers();

    }

    public async Task LoadCustomers()
    {
        var customers = await _customerService.GetAllAsync();
        foreach (var customer in customers)
        {
            CustomerReferenceModel customerReference = _customerMapper.ToReferenceModel(customer);
            Customers.Add(customerReference);
        }
    }

    #region navigationMethods
    [RelayCommand]
    public async Task GoToCustomerDetail(CustomerReferenceModel selectedCustomerReference)
    {
        var customerService = _serviceProvider.GetRequiredService<ICustomerService>();
        var selectedCustomerModel = await customerService.GetByIdAsync(selectedCustomerReference.Id);

        var customerDetailViewModel = _serviceProvider.GetRequiredService<CustomerDetailViewModel>();
        customerDetailViewModel.CurrentCustomer = selectedCustomerModel!;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = customerDetailViewModel;
    }

    [RelayCommand]
    public void GoToCustomerNew()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<CustomerNewViewModel>();
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
