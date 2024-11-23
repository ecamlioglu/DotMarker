using System.ComponentModel.DataAnnotations;
using DotMarker.Application.DTOs;
using DotMarker.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DotMarker.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody, Required] UserDto user)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid user data.");
            return BadRequest(ModelState);
        }

        var createdUser = await _userService.CreateUserAsync(user);
        return Ok(createdUser);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUserById(int userId)
    {
        var user = await _userService.GetUserByIdAsync(userId);
        return Ok(user);
    }

    [HttpPut("{userId}")]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody, Required] UserDto user)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid user data.");
            return BadRequest(ModelState);
        }

        await _userService.UpdateUserAsync(userId, user);
        return Ok();
    }

    [HttpGet("{userId}/contents")]
    public async Task<ActionResult<IEnumerable<ContentDto>>> GetUserContents(int userId)
    {
        var contents = await _userService.GetUserContentsAsync(userId);
        return Ok(contents);
    }

    [HttpPost("{userId}/contents/add")]
    public async Task<IActionResult> AddContent(int userId, [FromBody, Required] ContentDto content)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogWarning("Invalid content data.");
            return BadRequest(ModelState);
        }

        await _userService.AddContentAsync(userId, content);
        return Ok();
    }

    [HttpDelete("{userId}/contents/remove/{contentId}")]
    public async Task<IActionResult> RemoveContent(int userId, int contentId)
    {
        await _userService.RemoveContentAsync(userId, contentId);
        return Ok();
    }

    [HttpDelete("{userId}/contents/removeAll")]
    public async Task<IActionResult> RemoveAllContents(int userId)
    {
        await _userService.RemoveAllContentsAsync(userId);
        return Ok();
    }
}