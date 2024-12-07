using Todo.Core.Entities;

namespace Todo.Core.Interfaces;

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
    string GenerateToken(User user);

    /// <summary>
    ///     Gets the secret key for the token.
    /// </summary>
    /// <returns>
    ///     The secret key for the token.
    /// </returns>
    string GetSecretKey();

    /// <summary>
    ///     Gets the number of days until the token expires.
    /// </summary>
    /// <returns>
    ///     The number of days until the token expires.
    /// </returns>
    int GetTokenExpirationDays();
}