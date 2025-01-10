using System.Security.Claims;
using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Entities;

namespace Todo.Core.Interfaces;

/// <summary>
///     Interface for TokenService
/// </summary>
public interface ITokenService
{
    /// <summary>
    ///     Generate Token
    /// </summary>
    /// <param name="user">
    ///     User object to generate token for.
    /// </param>
    /// <returns>
    ///     AccessTokenDto object containing token and expiry date.
    /// </returns>
    AccessTokenDto GenerateToken(User user);

    /// <summary>
    ///     Generate Refresh Token.
    /// </summary>
    /// <returns>
    ///     RefreshTokenDto object containing refresh token and expiry date.
    /// </returns>
    RefreshTokenDto GenerateRefreshToken();

    /// <summary>
    ///     Get Principal from expired token.
    /// </summary>
    /// <param name="token">
    ///     Token to get principal from.
    /// </param>
    /// <returns>
    ///     ClaimsPrincipal object.
    /// </returns>
    ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
}