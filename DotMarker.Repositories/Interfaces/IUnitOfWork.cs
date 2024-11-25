namespace DotMarker.Repositories.Interfaces;

public interface IUnitOfWork
{
    IRepository<T> GetRepository<T>() where T : class;
    Task SaveAsync();
}