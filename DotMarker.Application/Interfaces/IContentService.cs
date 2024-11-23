using DotMarker.Application.DTOs;

namespace DotMarker.Application.Interfaces;

public interface IContentService
{
    Task<IEnumerable<ContentDto>> FilterContentsByCategoriesAsync(int[] categoryIds);
    Task<ContentDto> GetContentByVariantAsync(int contentId, int variantId);
    Task<VariantDto> AddVariantAsync(int contentId, VariantDto variant);
    Task RemoveVariantAsync(int contentId, int variantId);
    Task RemoveAllVariantsAsync(int contentId);
}