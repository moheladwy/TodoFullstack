using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AccountDTOs;

public class ChangePasswordDto
{
    [Required]
    public string Id { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string CurrentPassword { get; set; }

    [Required]
    [MinLength(12)]
    [MaxLength(100)]
    [DataType(DataType.Password)]
    [RegularExpression(
        @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*()_+\-=[\]{};':""\\|,.<>\/?]).{12,100}$",
        ErrorMessage = "Password must be at least 12 characters long, contain at least one uppercase letter, " +
                       "one lowercase letter, one number, and one special character."
    )]
    public string NewPassword { get; set; }
}