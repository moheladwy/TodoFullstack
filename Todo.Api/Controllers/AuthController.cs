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
    private readonly ITokenService _tokenService;
    private readonly ILogger<AuthController> _logger;

    /// <summary>
    ///     Constructor for the AuthController.
    /// </summary>
    /// <param name="authenticationService">
    ///     The service to use for authentication operations.
    /// </param>
    /// <param name="tokenService">
    ///     The service to use for token operations.
    /// </param>
    /// <param name="logger">
    ///     The logger to use for logging.
    /// </param>
    public AuthController(IAuthService authenticationService, ITokenService tokenService,
        ILogger<AuthController> logger)
    {
        _authenticationService = authenticationService;
        _tokenService = tokenService;
        _logger = logger;
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
        if (!ModelState.IsValid)
            throw new InvalidModelStateException($"Invalid model state because: {ModelState.ValidationState}");

        var user = await _authenticationService.Login(loginDto);
        var token = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        
        _logger.LogInformation("User logged in successfully with email: {email}", user.Email);
        return Ok(new AuthResponse
        {
            Id = user.Id,
            AccessToken = token.Token,
            AccessTokenExpirationDate = token.ExpirationDate,
            RefreshToken = refreshToken.RefreshToken,
            RefreshTokenExpirationDate = refreshToken.ExpirationDate
        });
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

        var user = await _authenticationService.Register(registerDto);
        var accessToken = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        _logger.LogInformation("User registered successfully with email: {email}", registerDto.Email);
        return Ok(new AuthResponse()
        {
            Id = user.Id,
            AccessToken = accessToken.Token,
            AccessTokenExpirationDate = accessToken.ExpirationDate,
            RefreshToken = refreshToken.RefreshToken,
            RefreshTokenExpirationDate = refreshToken.ExpirationDate
        });
    }
}