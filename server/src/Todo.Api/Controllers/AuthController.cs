using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;

namespace Todo.Api.Controllers;

/// <summary>
///     Controller for handling authentication operations includes login, register, and logout.
/// </summary>
/// <code>
///     - POST /api/auth/login
///     - POST /api/auth/refresh
///     - POST /api/auth/register
///     - POST /api/auth/logout
/// </code>
[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AuthController(
    IAuthService authenticationService,
    IAccountRepository accountRepository,
    ILogger<AuthController> logger,
    IValidator<LoginUserDto> loginUserValidator,
    IValidator<RegisterUserDto> registerUserValidator)
    : ControllerBase
{

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
    [AllowAnonymous]
    public async Task<IActionResult> Login([FromBody] LoginUserDto loginDto)
    {
        var validationResult = loginUserValidator.Validate(loginDto);
        if (!validationResult.IsValid)
            throw new InvalidModelStateException(validationResult.ToString());

        var response = await authenticationService.Login(loginDto);

        logger.LogInformation("User logged in successfully with Email: {Email}", loginDto.Email);
        return Ok(response);
    }

    /// <summary>
    ///     Login endpoint for users to authenticate themselves using refresh token and receive a JWT token for authorization using a refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for logging in the user.</param>
    /// <returns> An IActionResult representing the result of the login. </returns>
    /// <exception cref="InvalidModelStateException">Thrown when the model state is invalid.</exception>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginWithRefreshToken([FromBody] string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new InvalidModelStateException("Refresh Token is required");

        var response = await authenticationService.LoginWithRefreshToken(refreshToken);

        logger.LogInformation("User logged in successfully with Refresh Token");
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
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] RegisterUserDto registerDto)
    {
        var validationResult = registerUserValidator.Validate(registerDto);
        if (!validationResult.IsValid)
            throw new InvalidModelStateException(validationResult.ToString());

        var response = await authenticationService.Register(registerDto);

        logger.LogInformation("User registered successfully with email: {email}", registerDto.Email);
        return Ok(response);
    }

    /// <summary>
    ///     Logout endpoint for users to log out of the system.
    /// </summary>
    /// <returns> An IActionResult representing the result of the logout. </returns>
    /// <exception cref="InvalidModelStateException">Thrown when the model state is invalid.</exception>
    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        var user = await accountRepository.GetUserByClaims(User);
        if (user is null || string.IsNullOrEmpty(user.UserName))
            throw new InvalidModelStateException("User not found");

        await authenticationService.Logout(user.UserName);

        logger.LogInformation("User logged out successfully with username: {username}", user.UserName);
        return Ok();
    }
}