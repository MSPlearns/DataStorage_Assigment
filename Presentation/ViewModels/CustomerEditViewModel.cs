using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;

namespace Presentation.ViewModels;

public partial class CustomerEditViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [ObservableProperty]
    private Customer _currentCustomer = new();

    public Customer _originalCustomer = new();

    [ObservableProperty]
    private string _errorMessage= "";

    [ObservableProperty]
    private UpdateCustomerForm _upCustomerForm= new();

    [RelayCommand]
    public void Cancel()
    {
        var customerDetailViewModel = _serviceProvider.GetRequiredService<CustomerDetailViewModel>();
        customerDetailViewModel.CurrentCustomer = _originalCustomer;
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = customerDetailViewModel;
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        var validationResult = await ValidateForm();
        if (!validationResult)
            return;

        try
        {
        var customerService = _serviceProvider.GetRequiredService<ICustomerService>();
        await customerService.UpdateAsync(UpCustomerForm, CurrentCustomer);
    
            var customerDetailViewModel = _serviceProvider.GetRequiredService<CustomerDetailViewModel>();
            customerDetailViewModel.CurrentCustomer = await customerService.GetByIdAsync(CurrentCustomer.Id);
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = customerDetailViewModel;

        }
        catch (Exception)
        {
            ErrorMessage = "Error: Could not update customer.";
        }
    }

    #region validation
    private async Task<bool> ValidateForm()
    {
        ErrorMessage = "";
        bool isFormValid = true;
        var validationContext = new ValidationContext(new Customer());
        var validationResults = new List<ValidationResult>();
        var validationErrors = new List<string>();

        foreach (var property in typeof(UpdateCustomerForm).GetProperties())
        {
            validationContext.MemberName = property.Name;
            if (!Validator.TryValidateProperty(property.GetValue(UpCustomerForm), validationContext, validationResults))
            {
                isFormValid = false;
            }
        }

        if (UpCustomerForm.CustomerName != _originalCustomer.CustomerName &&
            await _serviceProvider.GetRequiredService<ICustomerService>().AlreadyExists(c => c.CustomerName == UpCustomerForm.CustomerName))
        {
            isFormValid = false;
            validationErrors.Add("A customer with the same name already exists.");
        }

        if (!isFormValid)
        {
            foreach (var error in validationResults)
            {
                validationErrors.Add(error.ErrorMessage); //validation method result  Error Message not the class ErrorMessage
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
