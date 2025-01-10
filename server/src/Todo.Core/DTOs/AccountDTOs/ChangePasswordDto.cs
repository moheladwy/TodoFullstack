using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.AccountDTOs;

/// <summary>
///     Data transfer object for changing a user's password.
/// </summary>
public class ChangePasswordDto
{
    /// <summary>
    ///     The user's unique identifier.
    /// </summary>
    public required string Id { get; set; }

    /// <summary>
    ///     The user's current password.
    /// </summary>
    public required string CurrentPassword { get; set; }

    /// <summary>
    ///     The user's new password.
    /// </summary>
    public required string NewPassword { get; set; }
}