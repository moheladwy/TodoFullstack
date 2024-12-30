namespace Todo.Core.DTOs.AuthDTOs;

/// <summary>
///     Data transfer object for refresh token.
/// </summary>
public class RefreshTokenDto
{
    /// <summary>
    ///     Gets or sets the refresh token.
    /// </summary>
    public string RefreshToken { get; set; } = string.Empty;

    /// <summary>
    ///     Gets or sets the refresh token expiration date.
    /// </summary>
    public DateTime RefreshTokenExpirationDate { get; set; }

    /// <summary>
    ///     Gets or sets the user ID.
    /// </summary>
    public string? UserId { get; set; }
}