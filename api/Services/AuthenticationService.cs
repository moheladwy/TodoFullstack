using Microsoft.AspNetCore.Identity;
using API.Exceptions;
using API.Interfaces;
using api.Models.DTOs.AuthDTOs;
using api.Models.Entities;

namespace API.Services;

/// <summary>
///     Service for handling user authentication operations in the system such as registering, logging in, and logging out.
/// </summary>
public class AuthenticationService : IAuthService
{
    /// <summary>
    ///     The UserManager instance to use for user management operations.
    /// </summary>
    private readonly UserManager<User> _userManager;

    /// <summary>
    ///     The SignInManager instance to use for user sign-in operations.
    /// </summary>
    private readonly SignInManager<User> _signInManager;

    /// <summary>
    ///     Constructor for the AuthenticationService class.
    /// </summary>
    /// <param name="userManager">
    ///     The UserManager instance to use for user management operations, it's registered in the DI container.
    /// </param>
    /// <param name="signInManager">
    ///     The SignInManager instance to use for user sign-in operations, it's registered in the DI container.
    /// </param>
    public AuthenticationService(
        UserManager<User> userManager,
        SignInManager<User> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
    public async Task<User> Register(RegisterUserDto registerUserDto)
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
        var result = await _userManager.CreateAsync(user, registerUserDto.Password);
        if (!result.Succeeded) 
            throw new CreateUserException($"Failed to create user because of: {string.Join(", ", result.Errors.Select(e => e.Description))}");

        return user;
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
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email) ??
                   throw new InvalidEmailException("Invalid email");

        var result = await _signInManager.CheckPasswordSignInAsync(user, loginUserDto.Password, false);
        if (!result.Succeeded) 
            throw new InvalidPasswordException($"Invalid password for the email {loginUserDto.Email} provided");
        
        return user;
    }
}