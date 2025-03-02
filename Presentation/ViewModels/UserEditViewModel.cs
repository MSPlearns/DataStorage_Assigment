using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System;


namespace Presentation.ViewModels;

public partial class UserEditViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;


    [ObservableProperty]
    private User _currentUser = new();

    [ObservableProperty]
    private string _errorMessage= "";

    [ObservableProperty]
    private UpdateUserForm _upUserForm = new();

    [RelayCommand]
    public void Cancel()
    {
        if (ErrorMessage == "")
        {
            var userDetailViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();
            userDetailViewModel.CurrentUser = CurrentUser;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = userDetailViewModel;
        }
        GoToUserList();
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        ErrorMessage = "";
        var userService = _serviceProvider.GetRequiredService<IUserService>();
        bool? result = await userService.UpdateAsync(UpUserForm, CurrentUser);

        if (result == true)
        {
            var userDetailViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();
            userDetailViewModel.CurrentUser = CurrentUser;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = userDetailViewModel;
        }
        else
        {
            ErrorMessage = "Error: Could not create user.";
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
