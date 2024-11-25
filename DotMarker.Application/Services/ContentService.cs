using DotMarker.Application.DTOs;
using DotMarker.Application.Interfaces;
using DotMarker.Domain.Entities;
using DotMarker.Infrastructure.Caching;
using DotMarker.Repositories.Interfaces;
using MapsterMapper;

namespace DotMarker.Application.Services;

public class ContentService : IContentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICacheManager _cacheManager;
    private readonly IMapper _dotmarkerMapper;

    public ContentService(IUnitOfWork unitOfWork, ICacheManager cacheManager, IMapper dotmarkerMapper)
    {
        _unitOfWork = unitOfWork;
        _cacheManager = cacheManager;
        _dotmarkerMapper = dotmarkerMapper;
    }

    public async Task<IEnumerable<ContentDto>> FilterContentsByCategoriesAsync(int[] categoryIds)
    {
        return await _cacheManager.GetOrSet(
            $"contents_categories_{string.Join("_", categoryIds)}",
            async () => _dotmarkerMapper.Map<IEnumerable<ContentDto>>(
                    await _unitOfWork.GetRepository<Content>().GetAllAsync())
                .Where(c => c.Category.Any(cat => categoryIds.Contains(cat))),
            TimeSpan.FromMinutes(15)
        );
    }

    public async Task<ContentDto> GetContentByVariantAsync(int contentId, int variantId)
    {
        var cacheKey = $"content_{contentId}_variant_{variantId}";

        return await _cacheManager.GetOrSet(
            cacheKey,
            async () =>
            {
                var content = await _unitOfWork.GetRepository<Content>()
                    .IncludeAsync(c => c.Variants, c => c.Id == contentId && c.Variants.Any(v => v.Id == variantId));


                return content != null ? _dotmarkerMapper.Map<ContentDto>(content) : null;
            },
            TimeSpan.FromMinutes(15)
        );
    }

    public async Task<VariantDto> AddVariantAsync(int contentId, VariantDto variant)
    {
        var content = await _unitOfWork.GetRepository<Content>().GetByIdAsync(contentId);
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
        var content = await _unitOfWork.GetRepository<Content>().GetByIdAsync(contentId);
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
        var content = await _unitOfWork.GetRepository<Content>().GetByIdAsync(contentId);
        if (content == null)
        {
            throw new Exception($"Content with ID {contentId} not found.");
        }

        content.RemoveAllVariants();
        await _unitOfWork.SaveAsync();
        _cacheManager.Remove($"content_{contentId}_variants");
    }
}