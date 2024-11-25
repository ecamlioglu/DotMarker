using DotMarker.Application.DTOs;
using DotMarker.Application.Interfaces;
using DotMarker.Domain.Entities;
using DotMarker.Infrastructure.Caching;
using DotMarker.Repositories.Interfaces;
using MapsterMapper;

namespace DotMarker.Application.Services;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheManager _cacheManager;
    private readonly IMapper _dotmarkerMapper;

    public UserService(IUnitOfWork unitOfWork, ICacheManager cacheManager, IMapper dotmarkerMapper)
    {
        _unitOfWork = unitOfWork;
        _cacheManager = cacheManager;
        _dotmarkerMapper = dotmarkerMapper;
    }

    public async Task<UserDto> CreateUserAsync(UserDto user)
    {
        var inputUser = _dotmarkerMapper.Map<User>(user);
        await _unitOfWork.GetRepository<User>().AddAsync(inputUser);
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"user_{inputUser.Id}");
        return user;
    }

    public async Task<IEnumerable<ContentDto>> GetUserContentsAsync(int userId)
    {
        return await _cacheManager.GetOrSet(
            $"user_{userId}_contents",
            async () =>
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                return _dotmarkerMapper.Map<IEnumerable<ContentDto>>(user?.Contents ?? Enumerable.Empty<Content>());
            },
            TimeSpan.FromMinutes(15)
        );
    }

    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        return await _cacheManager.GetOrSet(
            $"user_{userId}",
            async () =>
            {
                var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
                return _dotmarkerMapper.Map<UserDto>(user);
            },
            TimeSpan.FromMinutes(15)
        );
    }

    public async Task UpdateUserAsync(int userId, UserDto user)
    {
        var existingUser = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
        if (existingUser == null)
            throw new Exception($"User with ID {userId} not found.");

        _dotmarkerMapper.Map(user, existingUser);
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"user_{userId}");
    }

    public async Task AddContentAsync(int userId, ContentDto content)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
        if (user == null)
            throw new Exception($"User with ID {userId} not found.");

        var contentEntity = _dotmarkerMapper.Map<Content>(content);
        user.AddContent(contentEntity);
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"user_{userId}_contents");
    }

    public async Task RemoveContentAsync(int userId, int contentId)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
        if (user == null)
            throw new Exception($"User with ID {userId} not found.");

        var content = user.Contents.FirstOrDefault(c => c.Id == contentId);
        if (content == null)
            throw new Exception($"Content with ID {contentId} not found.");

        user.RemoveContent(content);
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"user_{userId}_contents");
    }

    public async Task RemoveAllContentsAsync(int userId)
    {
        var user = await _unitOfWork.GetRepository<User>().GetByIdAsync(userId);
        if (user == null)
            throw new Exception($"User with ID {userId} not found.");

        user.RemoveAllContents();
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"user_{userId}_contents");
    }
}