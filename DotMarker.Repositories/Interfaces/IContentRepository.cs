using DotMarker.Domain.Entities;

namespace DotMarker.Repositories.Interfaces;

public interface IContentRepository
{
    Task<IEnumerable<Content>> GetByCategoryAsync(int categoryId);
    Task<Content> GetContentWithVariantAsync(int contentId, int variantId);
    Task<Content> GetByIdAsync(int id);
    Task<IEnumerable<Content>> GetByCategoriesAsync(int[] categoryIds);
}