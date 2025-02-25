using Data.Context;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;
public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;

    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    //Create

    public virtual async Task<bool?> AddAsync(TEntity entity)
    {
        if (entity == null)
            return null!;

        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} entity:: {ex.Message}");
            return false;
        }
    }

    //Read

    //This method returns all entities of type TEntity.
    //If an includeExpression is provided, it will be used to include related entities.
    //If no entity is found, it will return an empty list.
    //for example if I want to get all projects and include the customer entity, I can pass a lambda expression that includes the customer entity
    public async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (includeExpression != null)
            query = includeExpression(query);

        return await query.ToListAsync();
    }

    //This method searches for an entity that matches the expression and returns it.
    //If a includedExpression is provided, it will be used to include related entities.
    //If the entity is not found, it will return null.
    //for example if I search for a specific proyect by project id and add customer to the include expression, it will return the project with the customer entity
    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (includeExpression != null)
            query = includeExpression(query);

        return await query.FirstOrDefaultAsync(expression);
    }

    //Update

    public async Task<bool?> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity)
    {
        if (updatedEntity == null)
            return null!;
        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(expression) ?? null!; 
            if (existingEntity == null)
                return null!;

            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(TEntity)} entity:: {ex.Message}");
            return false;
        }
    }

    //Delete
    public async Task<bool?> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return false;

        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(expression) ?? null!;
            if (existingEntity == null)
                return null;

            _dbSet.Remove(existingEntity);
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {nameof(TEntity)} entity:: {ex.Message}");
            return false;
        }
    }

    public virtual async Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }


}