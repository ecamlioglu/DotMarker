using DotMarker.Domain.Entities;
using DotMarker.Infrastructure.Data;
using DotMarker.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace DotMarker.Repositories.Operations;

public class ContentRepository : IContentRepository
{
    private readonly DotMarkerDatabaseContext _context;

    public ContentRepository(DotMarkerDatabaseContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Content>> GetByCategoryAsync(int categoryId)
    {
        return await _context.Contents.Where(c => c.CategoryId == categoryId).ToListAsync();
    }

    public async Task<Content> GetContentWithVariantAsync(int contentId, int variantId)
    {
        return await _context.Contents.Include(c => c.Variants)
            .FirstOrDefaultAsync(c => c.Id == contentId && c.Variants.Any(v => v.Id == variantId));
    }

    public async Task<Content> GetByIdAsync(int id)
    {
        return await _context.Contents.Include(c => c.Variants).FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<IEnumerable<Content>> GetByCategoriesAsync(int[] categoryIds)
    {
        return await _context.Contents.Include(c => c.Variants).Where(c => categoryIds.Contains(c.CategoryId)).ToListAsync();
    }
}