using DotMarker.Infrastructure.Data;
using DotMarker.Repositories.Interfaces;

namespace DotMarker.Repositories.UOW;

public class UnitOfWork : IUnitOfWork
{
    private readonly DotMarkerDatabaseContext _context;

    public IUserRepository UserRepository { get; }
    public IContentRepository ContentRepository { get; }

    public UnitOfWork(DotMarkerDatabaseContext context, IUserRepository userRepository,
        IContentRepository contentRepository)
    {
        _context = context;
        UserRepository = userRepository;
        ContentRepository = contentRepository;
    }

    public async Task SaveAsync()
    {
        await _context.SaveChangesAsync();
    }
}