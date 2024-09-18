using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using API.DTOs.AuthDTOs;
using API.Exceptions;
using API.Interfaces;
using API.Models;

namespace API.Services;

/// <summary>
///     Service for handling user authentication operations in the system such as registering, logging in, and logging out.
/// </summary>
public class AuthenticationService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly ILogger<AuthenticationService> _logger;

    /// <summary>
    ///     Constructor for the AuthenticationService class.
    /// </summary>
    /// <param name="userManager">
    ///     The UserManager instance to use for user management operations, it's registered in the DI container.
    /// </param>
    /// <param name="signInManager">
    ///     The SignInManager instance to use for user sign-in operations, it's registered in the DI container.
    /// </param>
    /// <param name="logger">
    ///    The ILogger instance to use for logging, it's registered in the DI container.
    ///    It's used to log information, warnings, and errors.
    /// </param>
    public AuthenticationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager,
        ILogger<AuthenticationService> logger)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
    }

    /// <summary>
    ///     Register a new user with the provided credentials.
    /// </summary>
    /// <param name="registerUserDto">
    ///     The credentials to use for registering a new user with the system, it includes the username, email, first name, last name, and password.
    /// </param>
    /// <returns>
    ///     A boolean indicating if the user was successfully registered into the system.
    /// </returns>
    /// <exception cref="CreateUserException">
    ///     Thrown when the user creation in the system fails.
    /// </exception>
    /// <exception cref="AddRoleException">
    ///     Thrown when the role `User` cannot be added to the user.
    /// </exception>
    public async Task<bool> Register(RegisterUserDto registerUserDto)
    {
        // Create a new user instance with the provided credentials.
        var user = new User
        {
            UserName = registerUserDto.Username,
            Email = registerUserDto.Email,
            FirstName = registerUserDto.FirstName,
            LastName = registerUserDto.LastName
        };

        // Create the user with the provided password
        var createdUser = await _userManager.CreateAsync(user, registerUserDto.Password);
        if (!createdUser.Succeeded)
        {
            var errorMessages = $"Failed to create user because of: {string.Join(", ", createdUser.Errors.Select(e => e.Description))}";
            _logger.LogError("{error}", errorMessages);
            throw new CreateUserException(errorMessages);
        }

        // Add Claim `User` to the user.
        await _userManager.AddClaimAsync(user, new Claim(Roles.User, Roles.User));

        var roleResult = await _userManager.AddToRoleAsync(user, Roles.User);
        if (!roleResult.Succeeded)
        {
            var errorMessages = $"Failed to add role `{Roles.User}` to the user because of: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}";
            _logger.LogError("{error}", errorMessages);
            throw new AddRoleException(errorMessages);
        }

        return true;
    }

    /// <summary>
    ///     Login a user with the provided credentials, which includes the email and password.
    /// </summary>
    /// <param name="loginUserDto">
    ///     The credentials to use for logging in as a user, it includes the email and password.
    /// </param>
    /// <returns>
    ///     The user that was successfully logged in.
    /// </returns>
    /// <exception cref="InvalidEmailException">
    ///     Thrown when the email provided is invalid.
    /// </exception>
    /// <exception cref="InvalidPasswordException">
    ///     Thrown when the password provided is invalid.
    /// </exception>
    public async Task<User> Login(LoginUserDto loginUserDto)
    {
        // Find the user by email
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email) ??
                   throw new InvalidEmailException("Invalid email");

        // Check if the password is correct
        var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
        if (!result.Succeeded)
        {
            var errorMessages = $"Failed to login user with email: {loginUserDto.Email}";
            _logger.LogError("{error}", errorMessages);
            throw new InvalidPasswordException(errorMessages);
        }

        // Sign in the user
        return user;
    }

    /// <summary>
    ///     Logout the current user from the system.
    /// </summary>
    /// <returns>
    ///     A boolean indicating if the user was successfully logged out of the system.
    /// </returns>
    /// <exception cref="NullReferenceException">
    ///     Thrown when the HttpContext is null.
    /// </exception>
    public async Task<bool> Logout()
    {
        await _signInManager.SignOutAsync();
        return true;
    }
}
