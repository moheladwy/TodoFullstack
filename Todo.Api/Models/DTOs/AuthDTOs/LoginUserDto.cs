namespace Todo.Api.Models.DTOs.AuthDTOs;

using System.ComponentModel.DataAnnotations;

public class LoginUserDto
{
    [Required]
    [EmailAddress]
    public required string Email { get; set; }

    [Required]
    [MinLength(12)]
    [MaxLength(100)]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':""\\|,.<>\/?]).{12,100}$",
        ErrorMessage = "Password must be at least 12 characters long, contain at least one uppercase letter, " +
                       "one lowercase letter, one number, and one special character."
    )]
    public required string Password { get; set; }
}