using Microsoft.AspNetCore.Identity;
using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Entities;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Task = System.Threading.Tasks.Task;

namespace Todo.Infrastructure.Services;

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
    ///     The TokenService instance to use for token operations.
    /// </summary>
    private readonly ITokenService _tokenService;

    /// <summary>
    ///     Constructor for the AuthenticationService class.
    /// </summary>
    /// <param name="userManager">
    ///     The UserManager instance to use for user management operations, it's registered in the DI container.
    /// </param>
    /// <param name="tokenService">
    ///     The TokenService instance to use for token operations, it's registered in the DI container.
    /// </param>
    public AuthenticationService(
        UserManager<User> userManager,
        ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
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
        var result = await _userManager.CheckPasswordAsync(user, loginUserDto.Password);

        if (!result)
            throw new InvalidPasswordException($"Invalid password for the email {loginUserDto.Email} provided");
        
        return user;
    }

    public async Task<AuthResponse> RefreshToken(RefreshTokenRequest request)
    {
        // var principal = _tokenService.GetPrincipalFromExpiredToken(request.AccessToken) ??
        //                 throw new InvalidTokenException("Invalid access token");
        //
        // var username = principal.Identity?.Name;
        // if (username == null) throw new InvalidTokenException("Invalid access token");
        //
        // var user = await _userManager.FindByNameAsync(username) ??
        //            throw new UserNotFoundException($"User with username {username} not found");
        //
        // if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpirationDate <= DateTime.UtcNow)
        //     throw new UnauthorizedAccessException("Invalid refresh token");
        //
        // var token = _tokenService.GenerateToken(user);
        // var refreshToken = _tokenService.GenerateRefreshToken();
        //
        // return new AuthResponse
        // {
        //     Id = user.Id,
        //     AccessToken = token.Token,
        //     AccessTokenExpirationDate = token.ExpirationDate,
        //     RefreshToken = refreshToken.RefreshToken,
        //     RefreshTokenExpirationDate = refreshToken.ExpirationDate
        // };
        throw new NotImplementedException();
    }

    /// <summary>
    ///     Logout a user with the provided username.
    /// </summary>
    /// <param name="username">
    ///     The username of the user to logout.
    /// </param>
    /// <exception cref="UserNotFoundException">
    ///     Thrown when the user with the provided username is not found in the system.
    /// </exception>
    public async Task Logout(string username)
    {
        var user = await _userManager.FindByNameAsync(username) ?? 
                   throw new UserNotFoundException($"User with username {username} not found");
        user.RefreshToken = null;
        await _userManager.UpdateAsync(user);
    }
}