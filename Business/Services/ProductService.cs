using Business.Mappers;
using Data.Entities;
using Data.Interfaces;
using Domain.Dtos;
using Domain.Factories;
using Domain.Models;

namespace Business.Services;

public class ProductService(IProductRepository productRepository, IProductFactory productFactory, IProductMapper productMapper) : IProductService
{
    private readonly IProductRepository _productRepository = productRepository;
    private readonly IProductFactory _productFactory = productFactory;
    private readonly IProductMapper _productMapper = productMapper;
    public async Task<bool?> AddAsync(CreateProductForm form)
    {
        Product productModel = _productFactory.FromForm(form);
        ProductEntity productEntity = _productMapper.ToEntity(productModel);
        return await _productRepository.AddAsync(productEntity);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        var result = await _productRepository.GetAllAsync();
        return result.Select(x => _productMapper.ToModel(x));
    }

    public async Task<Product?> GetByIdAsync(int id)
    {
        var result = await _productRepository.GetAsync(x => x.Id == id);
        if (result == null)
        {
            return null;
        }

        Product product = _productMapper.ToModel(result);
        return product;
    }

    public async Task<bool?> UpdateAsync(UpdateProductForm form, Product existingProduct)
    {
        existingProduct.ProductName = form.ProductName;
        existingProduct.Price = form.Price;
        existingProduct.Projects = form.Projects;
        var updatedEntity = _productMapper.ToEntity(existingProduct);

        return await _productRepository.UpdateAsync(x => x.Id == existingProduct.Id, updatedEntity);
    }
    public async Task<bool?> DeleteAsync(int id)
    {
        return await _productRepository.DeleteAsync(x => x.Id == id);
    }
}
