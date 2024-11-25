using DotMarker.Infrastructure.Data;
using DotMarker.Repositories.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DotMarker.Repositories.UOW;

public class UnitOfWork : IUnitOfWork, IDisposable, IAsyncDisposable
{
    private readonly DotMarkerDatabaseContext _context;
    private readonly IServiceProvider _serviceProvider;

    public UnitOfWork(DotMarkerDatabaseContext context, IServiceProvider serviceProvider)
    {
        _context = context;
        _serviceProvider = serviceProvider;
    }

    public IRepository<T> GetRepository<T>() where T : class
    {
        return _serviceProvider.GetRequiredService<IRepository<T>>();
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _context.Dispose();
        }
    }

    public async ValueTask DisposeAsync()
    {
        await DisposeAsyncCore();
        Dispose(false);
        GC.SuppressFinalize(this);
    }

    protected virtual async ValueTask DisposeAsyncCore()
    {
        await _context.DisposeAsync();
    }
}