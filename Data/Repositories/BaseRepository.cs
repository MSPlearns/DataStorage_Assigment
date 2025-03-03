using Data.Context;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Data.Repositories;
public abstract class BaseRepository<TEntity>(DataContext context) : IBaseRepository<TEntity> where TEntity : class
{
    protected readonly DataContext _context = context;

    protected readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    private IDbContextTransaction _transaction = null!;

    #region Transaction Management
    public virtual async Task BeginTransactionAsync() 
    {
        //Check if a transaction is already in progress
        //Initiates a new transaction if one is not already in progress
        _transaction ??= await _context.Database.BeginTransactionAsync();
    }
    public virtual async Task CommitTransactionAsync() 
    {
        //Checks that there is a transaction active
        if (_transaction != null)
        {
            //Commits the transaction and disposes of it
            await _transaction.CommitAsync();
            await _transaction.DisposeAsync();
            //Reset the transaction
            _transaction = null!; 
        }
    }

    public virtual async Task RollbackTransactionAsync() 
    {
        //Checks that there is a transaction active
        if (_transaction != null)
        {
            //Rollbacks the transaction and disposes of it
            await _transaction.RollbackAsync();
            await _transaction.DisposeAsync();
            //Reset the transaction
            _transaction = null!;
        }
    }

    #endregion Transaction Management


    #region CRUD Operations

    //Create

    public virtual async Task AddAsync(TEntity entity)
    {
            await _dbSet.AddAsync(entity);
    }

    //Read

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
        IQueryable<TEntity> query = _dbSet;
        if (includeExpression != null)
            query = includeExpression(query);

            return await query.ToListAsync();

    }

    public virtual async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null)
    {
       IQueryable<TEntity> query = _dbSet;
        if (includeExpression != null)
            query = includeExpression(query); 
        return await query.FirstOrDefaultAsync(expression);
    }

    //Update

    public virtual async Task UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity)
    {
        //removed null checks - checking in the service layer before calling the repository/DB instead
            var existingEntity = await _dbSet.FirstOrDefaultAsync(expression);
            if (existingEntity == null)
                throw new Exception();

            _context.Entry(existingEntity).CurrentValues.SetValues(updatedEntity);
    }

    //Delete
    public virtual void Delete(TEntity entity)
    {
        //removed null checks - checking in the service layer before calling the repository/DB instead
             _dbSet.Remove(entity);
    }

    //SaveChanges
    public virtual async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
    #endregion CRUD
    public virtual async Task<bool> EntityExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        return await _dbSet.AnyAsync(expression);
    }
}