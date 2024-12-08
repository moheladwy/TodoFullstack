namespace Todo.Core.DTOs.AuthDTOs;

/// <summary>
///     Represents the response of the authentication process.
/// </summary>
public class AuthResponse
{
    /// <summary>
    ///     The user's id.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    ///     The access token that will be used to authenticate the user.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    ///     The expiration date of the access token.
    /// </summary>
    public DateTime AccessTokenExpirationDate { get; set; }

    /// <summary>
    ///     The refresh token that will be used to refresh the access token.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    ///     The expiration date of the refresh token.
    /// </summary>
    public DateTime RefreshTokenExpirationDate { get; set; }
}