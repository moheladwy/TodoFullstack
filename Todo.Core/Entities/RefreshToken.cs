using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Todo.Core.Entities;

/// <summary>
///     Entity representing a refresh token in the system.
///   - Refresh tokens are used to obtain new access tokens when the current one expires.
/// </summary>
public class RefreshToken
{
    /// <summary>
    ///     The unique identifier of the refresh token.
    /// </summary>
    [Key]
    public Guid Id { get; set; }

    /// <summary>
    ///     The refresh token value.
    /// </summary>
    [DataType(DataType.Text)]
    [StringLength(200)]
    public required string Token { get; set; }

    /// <summary>
    ///     The expiration date of the refresh token.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime ExpirationDate { get; set; }

    /// <summary>
    ///     A foreign key to the user that the refresh token belongs to.
    /// </summary>
    [DataType(DataType.Text)]
    [ForeignKey("User")]
    public string? UserId { get; set; }

    /// <summary>
    ///     A navigation property to the user that the refresh token belongs to.
    /// </summary>
    public User User { get; set; }
}