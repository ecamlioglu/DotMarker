using DotMarker.Application.DTOs;
using DotMarker.Application.Interfaces;
using DotMarker.Domain.Entities;
using DotMarker.Infrastructure.Caching;
using DotMarker.Repositories.Interfaces;
using MapsterMapper;
using Microsoft.Extensions.Logging;

namespace DotMarker.Application.Services;

public class ContentService : IContentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheManager _cacheManager;
    private readonly IMapper _dotmarkerMapper;

    private readonly ILogger<ContentService> _logger;

    public ContentService(IUnitOfWork unitOfWork, ICacheManager cacheManager, IMapper dotmarkerMapper,
        ILogger<ContentService> logger)
    {
        _unitOfWork = unitOfWork;
        _cacheManager = cacheManager;
        _dotmarkerMapper = dotmarkerMapper;
        _logger = logger;
    }

    public async Task<IEnumerable<ContentDto>> FilterContentsByCategoriesAsync(int[] categoryIds)
    {
        _logger.LogInformation("Filtering contents by categories: {CategoryIds}", string.Join(", ", categoryIds));
        return await _cacheManager.GetOrSet(
            $"contents_categories_{string.Join("_", categoryIds)}",
            async () => _dotmarkerMapper.Map<IEnumerable<ContentDto>>(
                await _unitOfWork.ContentRepository.GetByCategoriesAsync(categoryIds)),
            TimeSpan.FromMinutes(15)
        );
    }

    public async Task<ContentDto> GetContentByVariantAsync(int contentId, int variantId)
    {
        var cacheKey = $"content_{contentId}_variant_{variantId}";
        _logger.LogInformation("Fetching content with ID {ContentId} and Variant ID {VariantId}", contentId, variantId);

        return await _cacheManager.GetOrSet(
            cacheKey,
            async () =>
            {
                var content = await _unitOfWork.ContentRepository.GetContentWithVariantAsync(contentId, variantId);
                if (content != null) return _dotmarkerMapper.Map<ContentDto>(content);
                _logger.LogWarning("Content with ID {ContentId} and Variant ID {VariantId} not found in repository.",
                    contentId, variantId);
                return null;
            },
            TimeSpan.FromMinutes(15)
        );
    }

    public async Task<VariantDto> AddVariantAsync(int contentId, VariantDto variant)
    {
        _logger.LogInformation("Adding variant for content ID {ContentId}", contentId);
        var content = await _unitOfWork.ContentRepository.GetByIdAsync(contentId);
        if (content == null)
        {
            throw new Exception($"Content with ID {contentId} not found.");
        }

        variant.ContentId = contentId;
        var variantEntity = _dotmarkerMapper.Map<Variant>(variant);
        content.AddVariant(variantEntity);

        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"content_{contentId}_variant_{variant.Id}");
        return variant;
    }

    public async Task RemoveVariantAsync(int contentId, int variantId)
    {
        _logger.LogInformation("Removing variant with ID {VariantId} for content ID {ContentId}", variantId, contentId);
        var content = await _unitOfWork.ContentRepository.GetByIdAsync(contentId);
        if (content == null)
        {
            throw new Exception($"Content with ID {contentId} not found.");
        }

        content.RemoveVariant(variantId);
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"content_{contentId}_variant_{variantId}");
    }

    public async Task RemoveAllVariantsAsync(int contentId)
    {
        _logger.LogInformation("Removing all variants for content ID {ContentId}", contentId);
        var content = await _unitOfWork.ContentRepository.GetByIdAsync(contentId);
        if (content == null)
        {
            throw new Exception($"Content with ID {contentId} not found.");
        }

        content.RemoveAllVariants();
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"content_{contentId}_variants");
    }
}