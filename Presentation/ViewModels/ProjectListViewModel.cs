using Business.Mappers;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;


namespace Presentation.ViewModels;

public partial class ProjectListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProjectService _projectService;
    private readonly IProjectMapper _projectMapper;


    [ObservableProperty]
    private ObservableCollection<ProjectReferenceModel> _projects = [];

    [ObservableProperty]
    private Project _selectedProject = null!;

    public ProjectListViewModel(IServiceProvider serviceProvider, IProjectService projectService, IProjectMapper projectMapper)
    {
        _serviceProvider = serviceProvider;
        _projectService = projectService;
        _projectMapper = projectMapper;
        LoadProjects();

    }

    public async Task LoadProjects()
    {
        var projects = await _projectService.GetAllAsync();
        foreach (var project in projects)
        {
            ProjectReferenceModel projectReference = _projectMapper.ToReferenceModel(project);
            Projects.Add(projectReference);
        }
    }

    [RelayCommand]
    public void GoToProjectDetail()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProjectDetailViewModel>();
    }

    [RelayCommand]
    public void GoToProjectNew()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProjectNewViewModel>();
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
