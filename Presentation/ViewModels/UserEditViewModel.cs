using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;


namespace Presentation.ViewModels;

public partial class UserEditViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;


    [ObservableProperty]
    private User _currentUser = new();

    public User _originalUser = new();

    [ObservableProperty]
    private string _formErrorMessage= "";

    [ObservableProperty]
    private UpdateUserForm _upUserForm = new();

    [RelayCommand]
    public void Cancel()
    {
        var userDetailViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();
        userDetailViewModel.CurrentUser = _originalUser;
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = userDetailViewModel;
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
        await userService.UpdateAsync(UpUserForm, CurrentUser);

            var userDetailViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();
            userDetailViewModel.CurrentUser = await userService.GetByIdAsync(CurrentUser.Id);
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = userDetailViewModel;
        }
        catch (Exception)
        {
            FormErrorMessage = "Error: Could not update user.";
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

        foreach (var property in typeof(UpdateUserForm).GetProperties())
        {
            validationContext.MemberName = property.Name;
            if (!Validator.TryValidateProperty(property.GetValue(UpUserForm), validationContext, validationResults))
                isFormValid = false;

        }

        if (UpUserForm.Email != _originalUser.Email &&
            await _serviceProvider.GetRequiredService<IUserService>().AlreadyExists(u => u.Email == UpUserForm.Email))
        {
            isFormValid = false;
            validationErrors.Add("A user with the same email already exists.");
        }

        if (!isFormValid)
        {
            foreach (var error in validationResults)
            {
                validationErrors.Add(error.ErrorMessage);
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
