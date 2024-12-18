using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.AuthDTOs;

/// <summary>
///     Data transfer object for user login.
/// </summary>
public class LoginUserDto
{
    /// <summary>
    ///     Email of the user.
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    ///     Password of the user.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 12)]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':""\\|,.<>\/?]).{12,100}$",
        ErrorMessage = "Password must be at least 12 characters long, contain at least one uppercase letter, " +
                       "one lowercase letter, one number, and one special character."
    )]
    public required string Password { get; set; }
}