using Business.Mappers;
using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
namespace Presentation.ViewModels;

public partial class ProductListViewModel : ObservableObject
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IProductService _productService;
    private readonly IProductMapper _productMapper;


    [ObservableProperty]
    private ObservableCollection<ProductReferenceModel> _products = [];

    [ObservableProperty]
    private Product _selectedProduct = null!;

    public ProductListViewModel(IServiceProvider serviceProvider, IProductService productService, IProductMapper productMapper)
    {
        _serviceProvider = serviceProvider;
        _productService = productService;
        _productMapper = productMapper;
        LoadProducts();

    }

    public async Task LoadProducts()
    {
        var products = await _productService.GetAllAsync();
        foreach (var product in products)
        {
            ProductReferenceModel productReference = _productMapper.ToReferenceModel(product);
            Products.Add(productReference);
        }
    }

    #region navigationMethods    
    [RelayCommand]
    public async Task GoToProductDetail(ProductReferenceModel selectedProductReference)
    {
        var productService = _serviceProvider.GetRequiredService<IProductService>();
        var selectedProductModel = await productService.GetByIdAsync(selectedProductReference.Id);

        var productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
        productDetailViewModel.CurrentProduct = selectedProductModel;

        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = productDetailViewModel;
    }

    [RelayCommand]
    public void GoToProductNew()
    {
        var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
        mainViewModel.CurrentViewModel = _serviceProvider.GetRequiredService<ProductNewViewModel>();
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
