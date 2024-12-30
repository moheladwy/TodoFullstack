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
    [Required]
    public required string Id { get; set; }

    /// <summary>
    ///     The user's current password.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 12)]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':""\\|,.<>\/?]).{12,100}$",
        ErrorMessage = "Password must be at least 12 characters long, contain at least one uppercase letter, " +
                       "one lowercase letter, one number, and one special character."
    )]
    public required string CurrentPassword { get; set; }

    /// <summary>
    ///     The user's new password.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 12)]
    [Compare("CurrentPassword", ErrorMessage = "New password must be different from the current password.")]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':""\\|,.<>\/?]).{12,100}$",
        ErrorMessage = "Password must be at least 12 characters long, contain at least one uppercase letter, " +
                       "one lowercase letter, one number, and one special character."
    )]
    public required string NewPassword { get; set; }
}