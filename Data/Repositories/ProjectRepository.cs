using Data.Context;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;

public class ProjectRepository(DataContext context) : BaseRepository<ProjectEntity>(context), IProjectRepository
{
    public  async Task<bool?> UpdateAsync(ProjectEntity updatedEntity, List<ProductEntity> newProducts)
    {
        var existingEntity = await _dbSet
            .Include(p => p.Products)
            .FirstOrDefaultAsync(p => p.Id == updatedEntity.Id);

        if (existingEntity == null)
            return null!;

        _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
        if (newProducts.Count != 0)
        {
            existingEntity.Products.Clear();
            foreach (var product in newProducts)
            {
                existingEntity.Products.Add(product);
            }
        }
        return true;
    }
}

