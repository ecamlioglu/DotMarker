using DotMarker.Application.DTOs;

namespace DotMarker.Application.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(UserDto user);
    Task<UserDto> GetUserByIdAsync(int userId);
    Task UpdateUserAsync(int userId, UserDto user);
    Task<IEnumerable<ContentDto>> GetUserContentsAsync(int userId);
    Task AddContentAsync(int userId, ContentDto content);
    Task RemoveContentAsync(int userId, int contentId);
    Task RemoveAllContentsAsync(int userId);
}