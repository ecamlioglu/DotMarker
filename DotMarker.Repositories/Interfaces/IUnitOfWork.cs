namespace DotMarker.Repositories.Interfaces;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IContentRepository ContentRepository { get; }
    Task SaveAsync();
}