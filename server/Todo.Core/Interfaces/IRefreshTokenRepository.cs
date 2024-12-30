using Todo.Core.DTOs.AuthDTOs;
using Todo.Core.Entities;
using Task = System.Threading.Tasks.Task;

namespace Todo.Core.Interfaces;

/// <summary>
///     Interface for the RefreshTokenRepository class.
/// </summary>
public interface IRefreshTokenRepository
{

    /// <summary>
    ///     Get a refresh token by the token string.
    /// </summary>
    /// <param name="refreshToken">
    ///     The refresh token string to use for finding the refresh token.
    /// </param>
    /// <returns>
    ///     The refresh token entity for the user if it exists, otherwise null.
    /// </returns>
    Task<RefreshToken?> GetRefreshTokenAsync(string refreshToken);

    /// <summary>
    ///     Get a refresh token by the user ID.
    /// </summary>
    /// <param name="userId">
    ///     The user ID to use for finding the refresh token.
    /// </param>
    /// <returns>
    ///     The refresh token entity for the user if it exists, otherwise null.
    /// </returns>
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(string userId);

    /// <summary>
    ///     Get a refresh token by the token ID.
    /// </summary>
    /// <param name="refreshTokenId">
    ///     The refresh token ID to use for finding the refresh token.
    /// </param>
    /// <returns>
    ///     The refresh token entity for the user if it exists, otherwise null.
    /// </returns>
    Task<RefreshToken?> GetRefreshTokenByIdAsync(Guid refreshTokenId);

    /// <summary>
    ///     Get a refresh token by the token string.
    /// </summary>
    /// <param name="refreshToken">
    ///     The refresh token string to use for finding the refresh token.
    /// </param>
    /// <returns>
    ///     Void.
    /// </returns>
    Task<RefreshToken> AddRefreshTokenAsync(RefreshTokenDto refreshToken);

    /// <summary>
    ///     Get a refresh token by the token string.
    /// </summary>
    /// <param name="refreshTokenId">
    ///     The refresh token ID to use for finding the refresh token.
    /// </param>
    /// <returns>
    ///     Void.
    /// </returns>
    Task DeleteRefreshTokenAsync(Guid refreshTokenId);
}