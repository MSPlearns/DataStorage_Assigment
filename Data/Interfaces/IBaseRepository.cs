using Data.Entities;
using System.Linq.Expressions;

namespace Data.Interfaces;

public interface IBaseRepository<TEntity> where TEntity : class
{
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();

    Task AddAsync(TEntity entity);
    Task<IEnumerable<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression=null);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> expression, Func<IQueryable<TEntity>, IQueryable<TEntity>>? includeExpression = null);
    Task UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity);
    void Delete(TEntity entity);
    Task<int> SaveAsync();
    Task<bool> EntityExistsAsync(Expression<Func<TEntity, bool>> expression);
}