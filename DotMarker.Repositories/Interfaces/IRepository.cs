using System.Linq.Expressions;

namespace DotMarker.Repositories.Interfaces;

public interface IRepository<T> where T : class
{
    Task<T> AddAsync(T entity);
    Task<T> GetByIdAsync(int id);
    Task<IEnumerable<T>> GetAllAsync();
    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
    void Update(T entity);
    void Remove(T entity);

    Task<T> IncludeAsync(Expression<Func<T, object>> includeExpression,
        Expression<Func<T, bool>> predicate);
    T Include(Func<IQueryable<T>, IQueryable<T>> includeFunc, Expression<Func<T, bool>> predicate);
}