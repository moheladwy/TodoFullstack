using System.Security.Claims;
using Todo.Api.Models.DTOs.AuthDTOs;
using Todo.Api.Models.Entities;

namespace Todo.Api.Interfaces;

/// <summary>
///     Interface for TokenService class.
/// </summary>
public interface ITokenService
{
    /// <summary>
    ///     Generates a token for a user based on the user's information.
    /// </summary>
    /// <param name="user">
    ///     The user to generate a token for.
    /// </param>
    /// <returns>
    ///     The generated token.
    /// </returns>
    AccessTokenDto GenerateToken(User user);
    
    /// <summary>
    ///     Generates a refresh token for a user based on the user's information.
    /// </summary>
    /// <returns>
    ///     The generated refresh token.
    /// </returns>
    RefreshTokenDto GenerateRefreshToken();
    
    /// <summary>
    ///     Gets the principal from an expired token.
    /// </summary>
    /// <param name="token">
    ///     The expired token to get the principal from.
    /// </param>
    /// <returns>
    ///     The principal from the expired token.
    /// </returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}