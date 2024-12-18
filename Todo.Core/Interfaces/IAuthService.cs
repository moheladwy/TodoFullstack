using Todo.Core.DTOs.AuthDTOs;
using Task = System.Threading.Tasks.Task;

namespace Todo.Core.Interfaces;

/// <summary>
/// Interface for authentication services.
/// </summary>
public interface IAuthService
{
    /// <summary>
    ///     Registers a new user with the provided credentials.
    /// </summary>
    /// <param name="registerUserDto">The data transfer object containing user registration details.</param>
    /// <returns>An <see cref="AuthResponse"/> indicating the result of the registration process.</returns>
    Task<AuthResponse> Register(RegisterUserDto registerUserDto);

    /// <summary>
    ///     Logs in a user with the provided credentials.
    /// </summary>
    /// <param name="loginUserDto">The data transfer object containing user login details.</param>
    /// <returns>An <see cref="AuthResponse"/> indicating the result of the login process.</returns>
    Task<AuthResponse> Login(LoginUserDto loginUserDto);

    /// <summary>
    ///     Logs in a user with the provided refresh token.
    /// </summary>
    /// <param name="refreshToken">The refresh token to use for logging in the user.</param>
    /// <returns>An <see cref="AuthResponse"/> indicating the result of the login process.</returns>
    Task<AuthResponse> LoginWithRefreshToken(string refreshToken);

    /// <summary>
    ///     Logs out a user with the provided username.
    /// </summary>
    /// <param name="username">The username of the user to log out.</param>
    /// <returns>A task representing the logout process.</returns>
    Task Logout(string username);
}