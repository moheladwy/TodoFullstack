using API.DTOs.AuthDTOs;
using API.Interfaces;
using API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

/// <summary>
///     Controller for handling authentication operations includes login, register, and logout.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{

    private readonly IAuthService _authenticationService;
    private readonly ITokenService _tokenService;

    /// <summary>
    ///     Constructor for the AuthController.
    /// </summary>
    /// <param name="authenticationService">
    ///     The service to use for authentication operations.
    /// </param>
    /// <param name="tokenService">
    ///     The service to use for token operations.
    /// </param>
    public AuthController(IAuthService authenticationService, ITokenService tokenService)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
    }

    /// <summary>
    ///     Login endpoint for users to authenticate themselves and receive a JWT token for authorization.
    /// </summary>
    /// <param name="loginDto">
    ///     The credentials to use for logging in as a user (email and password).
    /// </param>
    /// <returns>
    ///     An IActionResult representing the result of the login.
    ///     returns BadRequest if the ModelState is invalid or an exception is thrown,
    ///     otherwise returns Ok with the JWT token.
    /// </returns>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var user = await _authenticationService.Login(loginDto);
            return Ok(new AuthResponse
            {
                Id = user.Id,
                Username = user.UserName,
                Token = _tokenService.GenerateToken(user),
                ExpiresInDays = _tokenService.GetTokenExpirationDays()
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Register endpoint for users to create an account in the system.
    ///     The user will not be logged in after registering,
    ///     so he will need to log in after registering to receive a JWT token.
    /// </summary>
    /// <param name="registerDto">
    ///     The credentials to use for registering a new user (username, email, first name, last name, and password).
    /// </param>
    /// <returns>
    ///     An IActionResult representing the result of the registration.
    ///     returns BadRequest if the ModelState is invalid or an exception is thrown,
    ///     otherwise returns Ok.
    /// </returns>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            await _authenticationService.Register(registerDto);
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    /// <summary>
    ///     Logout endpoint for users to log out of the system.
    /// </summary>
    /// <returns>
    ///     An IActionResult representing the result of the logout.
    ///     returns Ok if the user was successfully logged out,
    ///     otherwise returns BadRequest.
    /// </returns>
    [HttpPost("logout")]
    [Authorize(Roles = Roles.User)]
    public async Task<ActionResult> Logout()
    {
        try
        {
            // BUG: The user is not logged out, the token is still valid and returns 404 Not Found.
            await _authenticationService.Logout();
            return Ok();
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
