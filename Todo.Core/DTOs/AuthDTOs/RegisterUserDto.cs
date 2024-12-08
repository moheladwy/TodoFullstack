using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.AuthDTOs;

/// <summary>
///     Data transfer object for registering a new user.
/// </summary>
public class RegisterUserDto
{
    /// <summary>
    ///     The email address of the user.
    /// </summary>
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    /// <summary>
    ///     The username of the user.
    /// </summary>
    [Required]
    public required string Username { get; set; }

    /// <summary>
    ///     The first name of the user.
    /// </summary>
    [Required]
    [StringLength(25, MinimumLength = 3)]
    public required string FirstName { get; set; }

    /// <summary>
    ///     The last name of the user.
    /// </summary>
    [Required]
    [StringLength(25, MinimumLength = 3)]
    public required string LastName { get; set; }

    /// <summary>
    ///     The password of the user.
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