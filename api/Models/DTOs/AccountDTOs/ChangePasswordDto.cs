namespace api.Models.DTOs.AccountDTOs;

using System.ComponentModel.DataAnnotations;

public class ChangePasswordDto
{
    [Required]
    public required string Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public required string CurrentPassword { get; set; }

    [Required]
    [MinLength(12)]
    [MaxLength(100)]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':""\\|,.<>\/?]).{12,100}$",
        ErrorMessage = "Password must be at least 12 characters long, contain at least one uppercase letter, " +
                       "one lowercase letter, one number, and one special character."
    )]
    public required string NewPassword { get; set; }
}