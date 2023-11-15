using DotnetAPI.DTOs;
using DotnetAPI.Helpers;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;
    private readonly AuthHelper _authHelper;
    public AuthController(
      ILogger<AuthController> logger,
      IUserService userService,
      IConfiguration configuration,
      IAuthService authService
    )
    {
        _userService = userService;
        _logger = logger;
        _authHelper = new(configuration);
        _authService = authService;
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Dictionary<string, string>>> Login([FromBody] LoginDTO loginDTO)
    {
        var token = await _authService.Login(loginDTO);
        if (token == null) return BadRequest("Invalid email or password.");
        return Ok(new Dictionary<string, string>{
      {"token", token},
    });
    }

    [HttpGet("RefreshToken")]
    public async Task<IActionResult> RefreshToken()
    {
        string? authUserId = User.FindFirst("UserId")?.Value;

        if (authUserId == null) return BadRequest("UserId cannot be converted into int type");
        User? user = await _userService.GetUser(int.Parse(authUserId));
        if (user == null) return BadRequest("User does not exist");

        var token = _authService.RefreshToken(authUserId, user.Role);
        if (token == null) return NotFound();
        return Ok(new Dictionary<string, string>{
      {"token", token},
    });
    }
}