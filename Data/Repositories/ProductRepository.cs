using Data.Context;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class ProductRepository(DataContext context) : BaseRepository<ProductEntity>(context), IProductRepository
{
    private readonly DataContext _context = context;

    public async Task<ProductEntity?> GetProductByIdAsync(int productId)
    {
        return await _dbSet.FirstOrDefaultAsync(x => x.Id == productId);
    }
}

