using DotnetAPI.Authorization;
using DotnetAPI.DTOs;
using DotnetAPI.Models;
using DotnetAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Web;

namespace DotnetAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
[AuthorizationLevel("Staff|Adm")]
public class UserController : ControllerBase
{

    private readonly ILogger<UserController> _logger;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;


    public UserController(
      IUserService userService,
      ILogger<UserController> logger,
      IAuthService authService
    )
    {
        _logger = logger;
        _authService = authService;
        _userService = userService;
    }

    [AllowAnonymous]
    [HttpPost("Resgister")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<ActionResult<User>> Register([FromBody] CreateUserDTO createUserDTO)
    {
        User? user = await _authService.Register(createUserDTO);
        // _constext.SaveChanges return the number of rows that were modified.
        if (user != null)
        {
            var actionName = nameof(GetUserByEmail);
            var routeValues = new { email = HttpUtility.UrlEncode(user?.Email) };
            return CreatedAtAction(actionName, routeValues, user);
        }

        throw new Exception("Error to Add this User");
    }

    [HttpGet("GetUserByEmail")]
    [ActionName("GetUserByEmail")]
    public async Task<ActionResult<User>> GetUserByEmail([FromQuery] EmailQueryParam query)
    {
        _logger.LogInformation($"GetUserByEmail has been called.");
        User? user = await _userService.GetUserByEmail(query.Email);
        if (user == null) return NotFound($"User email: {query.Email} not found");
        return Ok(user);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUserById(int id)
    {
        _logger.LogInformation("GetUsers has been called.");
        User? user = await _userService.GetUser(id);
        if (user == null) return DefaultNotFound(id);
        return Ok(user);
    }

    [HttpGet("")]
    public async Task<ActionResult<IEnumerable<User>>> GetUsers()
    {
        _logger.LogInformation("GetUsers has been called.");
        var users = await _userService.GetAllUsers();
        return Ok(users);
    }

    [HttpPatch]
    public async Task<ActionResult<User>> PatchUser([FromBody] UpdateUserDTO updateUserDTO)
    {
        _logger.LogInformation("PatchUsers has been called.");
        string? userId = User?.FindFirst("userId")?.Value;

        if (userId == null) return DefaultNotFound(0);

        User? user = await _userService.PatchUser(userId, updateUserDTO);
        if (user == null) return DefaultNotFound(int.Parse(userId));

        return Ok(user);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        _logger.LogInformation("DeleteUser has been called.");

        bool? deleteUser = await _userService.DeleteUser(id);

        if (deleteUser == null)
        {
            return DefaultNotFound(id);
        }
        if (deleteUser.Value)
        {
            return NoContent();
        };
        throw new Exception("Error to delete User");
    }
    protected NotFoundObjectResult DefaultNotFound(int id)
    {
        return NotFound("User id: " + id + " not found");
    }
}