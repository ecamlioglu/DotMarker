using DotMarker.Domain.Entities;
using DotMarker.Infrastructure.Data;
using DotMarker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotMarker.Repositories.Operations;

public class UserRepository: IUserRepository
{
    private readonly DotMarkerDatabaseContext _context;

    public UserRepository(DotMarkerDatabaseContext context)
    {
        _context = context;
    }

    public async Task<User> AddAsync(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<User> GetByIdAsync(int id)
    {
        return await _context.Users.Include(u => u.Contents).FirstOrDefaultAsync(u => u.Id == id);
    }
}