using DotMarker.Application.DTOs;
using DotMarker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace DotMarker.API.Controllers;

[ApiController]
[Route("api/contents")]
public class ContentController : ControllerBase
{
    private readonly IContentService _contentService;
    private readonly ILogger<ContentController> _logger;

    public ContentController(IContentService contentService, ILogger<ContentController> logger)
    {
        _contentService = contentService;
        _logger = logger;
    }

    [HttpGet("{category}")]
    public async Task<IActionResult> GetContentsByCategory(int[] categoryIds)
    {
        if (categoryIds == null || categoryIds.Length == 0)
        {
            _logger.LogWarning("Category IDs are null or empty.");
            return BadRequest("Category IDs are required.");
        }

        var contents = await _contentService.FilterContentsByCategoriesAsync(categoryIds);
        return Ok(contents);
    }

    [HttpGet("{contentId}/variants/{variantId}")]
    public async Task<IActionResult> GetContentVariant(int contentId, int variantId)
    {
        var content = await _contentService.GetContentByVariantAsync(contentId, variantId);
        return Ok(content);
    }

    [HttpPost("{contentId}/variants/add")]
    public async Task<IActionResult> AddVariant(int contentId, [FromBody] VariantDto variant)
    {
        if (variant == null)
        {
            _logger.LogWarning("Variant data is null.");
            return BadRequest("Variant data is required.");
        }

        var createdVariant = await _contentService.AddVariantAsync(contentId, variant);
        return Ok(createdVariant);
    }

    [HttpDelete("{contentId}/variants/{variantId}")]
    public async Task<IActionResult> RemoveVariant(int contentId, int variantId)
    {
        await _contentService.RemoveVariantAsync(contentId, variantId);
        return Ok();
    }

    [HttpDelete("{contentId}/variants/removeAll")]
    public async Task<IActionResult> RemoveAllVariants(int contentId)
    {
        await _contentService.RemoveAllVariantsAsync(contentId);
        return Ok();
    }
}