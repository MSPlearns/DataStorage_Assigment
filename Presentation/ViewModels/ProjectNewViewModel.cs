using Business.Mappers;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.ObjectModel;


namespace Presentation.ViewModels;

public partial class ProjectNewViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private Project _currentProject = new();

    [ObservableProperty]
    private string _errorMessage = "";

    [ObservableProperty]
    private CreateProjectForm _newProjectForm = new();

    [ObservableProperty]
    private ObservableCollection<UserReferenceModel> _availableUsers = new();
    [ObservableProperty]
    private ObservableCollection<CustomerReferenceModel> _availableCustomers = new();
    [ObservableProperty]
    private ObservableCollection<ProductReferenceModel> _availableProducts = new();
    [ObservableProperty]
    private ObservableCollection<String> _availableStatuses= new();

    [ObservableProperty]
    private ObservableCollection<ProductReferenceModel> _selectedProducts = new();
    public ProjectNewViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        LoadData();
    }

    public async Task LoadData()
    {
        await LoadAvailableUsers();
        await LoadAvailableCustomers();
        await LoadAvailableProducts();
        await LoadAvailableStatuses();
    }

    private async Task LoadAvailableStatuses()
    {
        var statusRepository = _serviceProvider.GetRequiredService<IStatusTypeRepository>();
        var statuses = await statusRepository.GetAllAsync();
        foreach (var status in statuses)
        {
            AvailableStatuses.Add(status.StatusName);
        }
    }

    private async Task LoadAvailableProducts()
    {
        var productService = _serviceProvider.GetRequiredService<IProductService>();
        var productMapper = _serviceProvider.GetRequiredService<IProductMapper>();
        var products = await productService.GetAllAsync();
        foreach (var product in products)
        {
            AvailableProducts.Add(productMapper.ToReferenceModel(product));
        }

    }

    private async Task LoadAvailableCustomers()
    {
        var customerService = _serviceProvider.GetRequiredService<ICustomerService>();
        var customerMapper = _serviceProvider.GetRequiredService<ICustomerMapper>();
        var customers = await customerService.GetAllAsync();
        foreach (var customer in customers)
        {
            AvailableCustomers.Add(customerMapper.ToReferenceModel(customer));
        }
    }

    private async Task LoadAvailableUsers()
    {
        var userService = _serviceProvider.GetRequiredService<IUserService>();
        var userMapper = _serviceProvider.GetRequiredService<IUserMapper>();
        var users = await userService.GetAllAsync();
        foreach (var user in users)
        {
            AvailableUsers.Add(userMapper.ToReferenceModel(user));
        }
    }

    [RelayCommand]
    public void Cancel()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProjectListViewModel>();
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        foreach (var product in AvailableProducts)
        {
            if (product.isSelected)
            {
                SelectedProducts.Add(product);
            }
        }
        NewProjectForm.AssociatedProducts = SelectedProducts.ToList();

        var projectService = _serviceProvider.GetRequiredService<IProjectService>();
        bool? result = await projectService.AddAsync(NewProjectForm);

        if (result == true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProjectListViewModel>();
        }
        else
        {
            ErrorMessage = "Error: Could not create project.";
        }
    }

    [RelayCommand]
public void ProductSelectionChanged(ProductReferenceModel selectedProduct)
    {
        if (selectedProduct.isSelected)
        {
            NewProjectForm.AssociatedProducts.Add(selectedProduct);
        }
        else
        {
            NewProjectForm.AssociatedProducts.Remove(selectedProduct);
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
