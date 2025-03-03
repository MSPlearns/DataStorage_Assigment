using Business.Services;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Dtos;
using Domain.Models;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.DataAnnotations;
namespace Presentation.ViewModels;

public partial class ProductEditViewModel(IServiceProvider serviceProvider) : ObservableObject
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;


    [ObservableProperty]
    private Product _currentProduct = new();
    public Product _originalProduct = new();

    [ObservableProperty]
    private string _errorMessage = "";

    [ObservableProperty]
    private UpdateProductForm _upProductForm = new();

    [RelayCommand]
    public void Cancel()
    {
        if (ErrorMessage == "")
        {
            var productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
            productDetailViewModel.CurrentProduct = _originalProduct;
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = productDetailViewModel;
        }
        GoToProductList();
    }

    [RelayCommand]
    public async Task SaveChanges()
    {
        var validationResult = await ValidateForm();
        if (!validationResult)
            return;

        try
        {
            var productService = _serviceProvider.GetRequiredService<IProductService>();
            await productService.UpdateAsync(UpProductForm, CurrentProduct);

            var productDetailViewModel = _serviceProvider.GetRequiredService<ProductDetailViewModel>();
            productDetailViewModel.CurrentProduct = await productService.GetByIdAsync(CurrentProduct.Id);
            var mainViewModel = _serviceProvider.GetRequiredService<MainViewModel>();
            mainViewModel.CurrentViewModel = productDetailViewModel;
        }
        catch (Exception)
        {
            ErrorMessage = "Error: Could not create product.";
        }
    }

    #region validation
    private async Task<bool> ValidateForm()
    {
        ErrorMessage = "";
        bool isFormValid = true;
        var validationContext = new ValidationContext(new Product());
        var validationResults = new List<ValidationResult>();
        var validationErrors = new List<string>();

        foreach (var property in typeof(UpdateProductForm).GetProperties())
        {
            validationContext.MemberName = property.Name;
            if (!Validator.TryValidateProperty(property.GetValue(UpProductForm), validationContext, validationResults))
            {
                isFormValid = false;
            }
        }
        if (UpProductForm.ProductName != _originalProduct.ProductName &&
            await _serviceProvider.GetRequiredService<IProductService>().AlreadyExists(p => p.ProductName == UpProductForm.ProductName))
        {
            isFormValid = false;
            validationErrors.Add("A product with the same name already exists.");
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