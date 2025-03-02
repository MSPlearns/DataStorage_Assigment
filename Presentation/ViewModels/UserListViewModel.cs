using Business.Mappers;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Data.Repositories;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;


namespace Presentation.ViewModels;

public partial class UserListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IUserService _userService;
    private readonly IUserMapper _userMapper;


    [ObservableProperty]
    private ObservableCollection<UserReferenceModel> _users = [];

    [ObservableProperty]
    private User _selectedUser = null!;

    public UserListViewModel(IServiceProvider serviceProvider, IUserService userService, IUserMapper userMapper)
    {
        _serviceProvider = serviceProvider;
        _userService = userService;
        _userMapper = userMapper;
        LoadUsers();
    }

    public async Task LoadUsers()
    {

        var users = await _userService.GetAllAsync();
        foreach (var user in users)
        {
            UserReferenceModel userref = _userMapper.ToReferenceModel(user);
            Users.Add(userref);
        }
    }

    [RelayCommand]
    public async Task GoToUserDetail(UserReferenceModel selectedUserReference)
    {
        var userService = _serviceProvider.GetRequiredService<IUserService>();
        var selectedUserModel = await userService.GetByIdAsync(selectedUserReference.Id);

        var userDetailViewModel = _serviceProvider.GetRequiredService<UserDetailViewModel>();
        userDetailViewModel.CurrentUser = selectedUserModel;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = userDetailViewModel;
    }

    [RelayCommand]
    public void GoToUserNew()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserNewViewModel>();
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
