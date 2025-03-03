using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;


namespace Presentation.ViewModels;

public partial class UserNewViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [ObservableProperty]
    private string _formErrorMessage = "";

    [ObservableProperty]
    private CreateUserForm _newUserForm = new();

    [RelayCommand]
    public void Cancel()
    {
        GoToUserList();
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        var validationResult = await ValidateForm();
        if (!validationResult)
            return;

        try
        {
            var userService = _serviceProvider.GetRequiredService<IUserService>();
            await userService.AddAsync(NewUserForm);
            GoToUserList();
        }
        catch (Exception)
        {
            FormErrorMessage = "Error: Could not create user.";
        }

    }

    #region validation
    private async Task<bool> ValidateForm()
    {
        FormErrorMessage = "";
        bool isFormValid = true;
        var validationContext = new ValidationContext(new User());
        var validationResults = new List<ValidationResult>();
        var validationErrors = new List<string>();

        foreach (var property in typeof(CreateUserForm).GetProperties())
        {
            validationContext.MemberName = property.Name;
            if (!Validator.TryValidateProperty(property.GetValue(NewUserForm), validationContext, validationResults))
            {
                isFormValid = false;
            }
        }

        if (await _serviceProvider.GetRequiredService<IUserService>().AlreadyExists(u => u.Email == NewUserForm.Email))
        {
            isFormValid = false;
            validationErrors.Add("A user with the same email already exists.");
        }

        if (!isFormValid)
        {
            foreach (var error in validationResults)
            {
                validationErrors.Add(error.ErrorMessage!);
            }
        }
        FormErrorMessage = string.Join(Environment.NewLine, validationErrors);
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
