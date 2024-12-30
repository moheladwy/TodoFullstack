namespace Todo.Core.DTOs.AuthDTOs;

/// <summary>
///     Access token data transfer object.
/// </summary>
public class AccessTokenDto
{
    /// <summary>
    ///     User id.
    /// </summary>
    public string UserId { get; set; } = string.Empty;

    /// <summary>
    ///     Access token.
    /// </summary>
    public string AccessToken { get; set; } = string.Empty;

    /// <summary>
    ///     Access token expiration date.
    /// </summary>
    public DateTime AccessTokenExpirationDate { get; set; }
}