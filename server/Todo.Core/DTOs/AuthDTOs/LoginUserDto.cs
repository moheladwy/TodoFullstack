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
    public required string Email { get; set; }

    /// <summary>
    ///     Password of the user.
    /// </summary>
    public required string Password { get; set; }
}