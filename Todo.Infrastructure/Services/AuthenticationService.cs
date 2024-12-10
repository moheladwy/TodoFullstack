using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Entities;
using Todo.Core.Exceptions;
using Todo.Core.Interfaces;
using Todo.Infrastructure.DatabaseContexts;
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
    ///     The RefreshTokenRepository instance to use for refresh token operations.
    /// </summary>
    private readonly IRefreshTokenRepository _refreshTokenRepository;

    /// <summary>
    ///     The TodoIdentityContext instance to use for database operations.
    /// </summary>
    private readonly TodoIdentityContext _context;

    /// <summary>
    ///     Constructor for the AuthenticationService class.
    /// </summary>
    /// <param name="userManager">
    ///     The UserManager instance to use for user management operations, it's registered in the DI container.
    /// </param>
    /// <param name="tokenService">
    ///     The TokenService instance to use for token operations, it's registered in the DI container.
    /// </param>
    /// <param name="context">
    ///     The TodoIdentityContext instance to use for database operations, it's registered in the DI container.
    /// </param>
    /// <param name="refreshTokenRepository">
    ///     The RefreshTokenRepository instance to use for refresh token operations, it's registered in the DI container.
    /// </param>
    public AuthenticationService(
        UserManager<User> userManager,
        ITokenService tokenService,
        TodoIdentityContext context,
        IRefreshTokenRepository refreshTokenRepository)
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _context = context;
        _refreshTokenRepository = refreshTokenRepository;
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
    public async Task<AuthResponse> Register(RegisterUserDto registerUserDto)
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

        var token = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();
        refreshToken.UserId = user.Id;

        var refreshTokenEntity = await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);
        await UpdateUserRefreshToken(user, refreshTokenEntity);

        return GenerateAuthResponse(user, token, refreshToken);
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
    public async Task<AuthResponse> Login(LoginUserDto loginUserDto)
    {
        var user = await _userManager.FindByEmailAsync(loginUserDto.Email) ??
                   throw new InvalidEmailException("Invalid email");
        if (!await _userManager.CheckPasswordAsync(user, loginUserDto.Password))
            throw new InvalidPasswordException($"Invalid password for the email {loginUserDto.Email} provided");
        
        var token = _tokenService.GenerateToken(user);
        var refreshToken = _tokenService.GenerateRefreshToken();

        if (user.RefreshToken == null || user.RefreshToken?.ExpirationDate <= DateTime.UtcNow)
        {
            refreshToken.UserId = user.Id;

            var refreshTokenEntity = await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken);

            await UpdateUserRefreshToken(user, refreshTokenEntity);
        }

        return GenerateAuthResponse(user, token, refreshToken);
    }

    // BUG: The method throws an exception when trying to fetch the user with the provided refresh token.
    /// <summary>
    ///     Login a user with the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for logging in the user.</param>
    /// <returns>An <see cref="AuthResponse"/> indicating the result of the login process.</returns>
    public async Task<AuthResponse> LoginWithRefreshToken(string refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken))
            throw new ArgumentNullException(nameof(refreshToken), "Refresh token cannot be null or empty");

        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.RefreshToken.Token == refreshToken);

        if (user == null || user.RefreshToken?.ExpirationDate <= DateTime.UtcNow)
            throw new UnauthorizedAccessException("Invalid refresh token");

        var token = _tokenService.GenerateToken(user);
        var newRefreshToken = _tokenService.GenerateRefreshToken();
        newRefreshToken.UserId = user.Id;

        var refreshTokenEntity = await _refreshTokenRepository.AddRefreshTokenAsync(newRefreshToken);
        await UpdateUserRefreshToken(user, refreshTokenEntity);

        return GenerateAuthResponse(user, token, newRefreshToken);
    }

    private async Task UpdateUserRefreshToken(User user, RefreshToken refreshToken)
    {
        user.RefreshTokenId = refreshToken.Id;
        await _userManager.UpdateAsync(user);
    }

    private static AuthResponse GenerateAuthResponse(User user, AccessTokenDto token, RefreshTokenDto refreshToken)
    {
        return new AuthResponse
        {
            UserId = user.Id,
            AccessToken = token.AccessToken,
            AccessTokenExpirationDate = token.AccessTokenExpirationDate,
            RefreshToken = refreshToken.RefreshToken,
            RefreshTokenExpirationDate = refreshToken.RefreshTokenExpirationDate
        };
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