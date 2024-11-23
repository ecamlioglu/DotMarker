using DotMarker.Domain.Entities;

namespace DotMarker.Repositories.Interfaces;

public interface IUserRepository
{
    Task<User> AddAsync(User user);
    Task<User> GetByIdAsync(int id);
}