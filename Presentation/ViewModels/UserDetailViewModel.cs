using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;


namespace Presentation.ViewModels;

public partial class UserDetailViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    [ObservableProperty]
    private User _currentUser = new();

    [RelayCommand]

    public async Task DeleteUser()
    {
        var userService = _serviceProvider.GetRequiredService<IUserService>();
        bool? result = await userService.DeleteAsync(CurrentUser.Id);
        if (result == true)
        {
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<UserListViewModel>();
        }

    }

    [RelayCommand]
    public void GoToUserEdit()
    {
        var userEditViewModel = _serviceProvider.GetRequiredService<UserEditViewModel>();
        userEditViewModel.CurrentUser = CurrentUser;
        userEditViewModel._originalUser = CurrentUser;
        userEditViewModel.UpUserForm.FirstName = CurrentUser.FirstName;
        userEditViewModel.UpUserForm.LastName = CurrentUser.LastName;
        userEditViewModel.UpUserForm.Email = CurrentUser.Email;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = userEditViewModel;
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
