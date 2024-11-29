using api.Models.DTOs.AuthDTOs;
using api.Models.Entities;

namespace API.Interfaces;

/// <summary>
///     Interface for the AuthService.
/// </summary>
public interface IAuthService
{
    /// <summary>
    ///     Register a new user with the provided RegisterUserDto.
    /// </summary>
    /// <param name="registerUserDto">
    ///     The RegisterUserDto containing the user's information.
    /// </param>
    /// <returns>
    ///     A boolean indicating whether the registration was successful.
    /// </returns>
    Task<bool> Register(RegisterUserDto registerUserDto);

    /// <summary>
    ///     Log in a user with the provided LoginUserDto.
    /// </summary>
    /// <param name="loginUserDto">
    ///     The LoginUserDto containing the user's information.
    /// </param>
    /// <returns>
    ///     The User object of the logged-in user.
    /// </returns>
    Task<User> Login(LoginUserDto loginUserDto);
}