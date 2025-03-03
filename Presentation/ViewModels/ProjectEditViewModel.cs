using Business.Mappers;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

public partial class ProjectEditViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;

    [ObservableProperty]
    private Project _currentProject = new();

    public Project _originalProject = new();

    [ObservableProperty]
    private string _errorMessage = "";

    [ObservableProperty]
    private UpdateProjectForm _upProjectForm = new();

    [ObservableProperty]
    private ObservableCollection<UserReferenceModel> _availableUsers = [];
    [ObservableProperty]
    private ObservableCollection<CustomerReferenceModel> _availableCustomers = [];
    [ObservableProperty]
    private ObservableCollection<ProductReferenceModel> _availableProducts = [];
    [ObservableProperty]
    private ObservableCollection<String> _availableStatuses = [];

    [ObservableProperty]
    private ObservableCollection<ProductReferenceModel> _selectedProducts = [];
    public ProjectEditViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        LoadData();
    }

    #region LoadDataMethods

    public async Task LoadData()
    {
        await LoadAvailableUsers();
        await LoadAvailableCustomers();
        await LoadAvailableProducts();
        await LoadAvailableStatuses();
        await UpdateIsSelectedInAvailableProductsAsync(); 
        //This is not working, it does what it is supposed to do in the background but it is not displaying in the view
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

    public async Task UpdateIsSelectedInAvailableProductsAsync()
    {
        foreach (var product in AvailableProducts)
        {
            if (UpProjectForm.AssociatedProducts.Contains(product))
            {
                product.isSelected = true;
            }
            else
            {
                product.isSelected = false;
            }
        }
    }
    #endregion LoadDataMethods



    [RelayCommand]
    public void Cancel()
    {
        if (ErrorMessage == "")
        {
            var projectDetailViewModel = _serviceProvider.GetRequiredService<ProjectDetailViewModel>();
            projectDetailViewModel.CurrentProject = _originalProject;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = projectDetailViewModel;
        }
        GoToProjectList();
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        SyncSelectedProducts();

        var validationResult = await ValidateForm();
        if (!validationResult)
            return;

        try
        {
            var projectService = _serviceProvider.GetRequiredService<IProjectService>();
            await projectService.UpdateAsync(UpProjectForm, CurrentProject);

            var projectDetailViewModel = _serviceProvider.GetRequiredService<ProjectDetailViewModel>();
            projectDetailViewModel.CurrentProject = await projectService.GetByIdAsync(CurrentProject.Id);
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = projectDetailViewModel;
        }
        catch (Exception)
        {
            ErrorMessage = "Error: Could not update project.";

        }
    }

    private async Task SyncSelectedProducts()
    {
        foreach (var product in AvailableProducts)
        {
            if (product.isSelected)
            {
                SelectedProducts.Add(product);
            }
            else
            {
                SelectedProducts.Remove(product);
            }
        }
        UpProjectForm.AssociatedProducts = SelectedProducts.ToList();
    }

    [RelayCommand]
    public void ProductSelectionChanged(ProductReferenceModel selectedProduct)
    {
        if (selectedProduct.isSelected)
        {
            UpProjectForm.AssociatedProducts.Add(selectedProduct);
        }
        else
        {
            UpProjectForm.AssociatedProducts.Remove(selectedProduct);
        }
    }

    #region validation
    private async Task<bool> ValidateForm()
    {
        ErrorMessage = "";
        bool isFormValid = true;
        var validationContext = new ValidationContext(new Project());
        var validationResults = new List<ValidationResult>();
        var validationErrors = new List<string>();

        foreach (var property in typeof(UpdateProjectForm).GetProperties())
        {
            validationContext.MemberName = property.Name;
            if (!Validator.TryValidateProperty(property.GetValue(UpProjectForm), validationContext, validationResults))
            {
                isFormValid = false;
            }
        }

        if (UpProjectForm.AssociatedProducts.Count == 0) //Custom validation to make sure at least one product is selected
        {
            isFormValid = false;
            validationErrors.Add("At least one product must be selected.");
        }

        if (UpProjectForm.Title != _originalProject.Title &&
            await _serviceProvider.GetRequiredService<IProjectService>().AlreadyExists(p => p.Title == UpProjectForm.Title))

        {
            isFormValid = false;
            validationErrors.Add("A project with the same title already exists.");
        }

        if (!isFormValid)
        {
            foreach (var error in validationResults)
            {
                validationErrors.Add(error.ErrorMessage!); //validation method result  Error Message not the class ErrorMessage
            }
        }
        ErrorMessage = string.Join(Environment.NewLine, validationErrors);
        return isFormValid;
    }
    #endregion validation

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

