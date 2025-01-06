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
    public required string Id { get; init; }

    /// <summary>
    ///     The user's first name.
    /// </summary>
    public string? NewFirstName { get; set; }

    /// <summary>
    ///     The user's last name.
    /// </summary>
    public string? NewLastName { get; set; }

    /// <summary>
    ///     The user's email.
    /// </summary>
    public string? NewEmail { get; set; }

    /// <summary>
    ///     The user's username.
    /// </summary>
    public string? NewUserName { get; set; }

    /// <summary>
    ///     The user's phone number.
    /// </summary>
    [Phone]
    public string? NewPhoneNumber { get; set; }
}