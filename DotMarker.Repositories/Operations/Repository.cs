using DotMarker.Infrastructure.Data;
using DotMarker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotMarker.Repositories.Operations;

public class Repository<T>(DotMarkerDatabaseContext context) : IRepository<T>
    where T : class
{
    public async Task<T> AddAsync(T entity)
    {
        await context.Set<T>().AddAsync(entity);
        return entity;
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await context.Set<T>().FindAsync(id) ?? throw new InvalidOperationException();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await context.Set<T>().ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Where(predicate).ToListAsync();
    }

    public void Update(T entity)
    {
        context.Set<T>().Update(entity);
    }

    public void Remove(T entity)
    {
        context.Set<T>().Remove(entity);
    }

    public async Task<T> IncludeAsync(Expression<Func<T, object>> includeExpression, Expression<Func<T, bool>> predicate)
    {
        return await context.Set<T>().Include(includeExpression).FirstOrDefaultAsync(predicate) ?? throw new InvalidOperationException();
    }

    public T Include(Func<IQueryable<T>, IQueryable<T>> includeFunc, Expression<Func<T, bool>> predicate)
    {
        return includeFunc(context.Set<T>()).FirstOrDefault(predicate) ?? throw new InvalidOperationException();
    }
}