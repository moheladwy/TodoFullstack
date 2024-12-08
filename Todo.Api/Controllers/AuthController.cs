using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;

namespace Todo.Api.Controllers;

/// <summary>
///     Controller for handling authentication operations includes login, register, and logout.
/// </summary>
[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authenticationService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    ///     Constructor for the AuthController.
    /// </summary>
    /// <param name="authenticationService">
    ///     The service to use for authentication operations.
    /// </param>
    /// <param name="logger">
    ///     The logger to use for logging.
    /// </param>
    public AuthController(IAuthService authenticationService, ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _logger = logger;
    }

    /// <summary>
    ///     Login endpoint for users to authenticate themselves using email and password, and receive a JWT token for authorization.
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
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state because: {ModelState.ValidationState}");

        var response = await _authenticationService.Login(loginDto);

        _logger.LogInformation("User logged in successfully with Email: {Email}", loginDto.Email);
        return Ok(response);
    }

    /// <summary>
    ///     Login endpoint for users to authenticate themselves using refresh token and receive a JWT token for authorization using a refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for logging in the user.</param>
    /// <returns> An IActionResult representing the result of the login. </returns>
    /// <exception cref="InvalidModelStateException">Thrown when the model state is invalid.</exception>
    [HttpPost("refresh")]
    public async Task<IActionResult> LoginWithRefreshToken([FromBody] string refreshToken)
    {
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state because: {ModelState.ValidationState}");

        var response = await _authenticationService.LoginWithRefreshToken(refreshToken);

        _logger.LogInformation("User logged in successfully with Refresh Token");
        return Ok(response);
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
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state because: {ModelState.ValidationState}");

        var response = await _authenticationService.Register(registerDto);

        _logger.LogInformation("User registered successfully with email: {email}", registerDto.Email);
        return Ok(response);
    }

    /// <summary>
    ///     Logout endpoint for users to log out of the system.
    /// </summary>
    /// <param name="username">The username of the user to log out.</param>
    /// <returns> An IActionResult representing the result of the logout. </returns>
    /// <exception cref="InvalidModelStateException">Thrown when the model state is invalid.</exception>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout([FromBody] string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new InvalidModelStateException("Username is required");

        await _authenticationService.Logout(username);

        _logger.LogInformation("User logged out successfully with username: {username}", username);
        return Ok();
    }
}