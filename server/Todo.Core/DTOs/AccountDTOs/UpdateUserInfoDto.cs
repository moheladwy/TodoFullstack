using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.AccountDTOs;

/// <summary>
///     DTO for updating user information.
/// </summary>
public class UpdateUserInfoDto
{
    /// <summary>
    ///     The user's id.
    /// </summary>
    [Required]
    public required string Id { get; init; }

    /// <summary>
    ///     The user's first name.
    /// </summary>
    [StringLength(25, MinimumLength = 3)]
    public string? NewFirstName { get; set; }

    /// <summary>
    ///     The user's last name.
    /// </summary>
    [StringLength(25, MinimumLength = 3)]
    public string? NewLastName { get; set; }

    /// <summary>
    ///     The user's email.
    /// </summary>
    [EmailAddress]
    public string? NewEmail { get; set; }

    /// <summary>
    ///     The user's username.
    /// </summary>
    [StringLength(25, MinimumLength = 3)]
    public string? NewUserName { get; set; }

    /// <summary>
    ///     The user's phone number.
    /// </summary>
    [Phone]
    public string? NewPhoneNumber { get; set; }
}