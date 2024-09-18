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
    [
        RegularExpression("([a-z][A-Z][0-9][!@#$%^&*()_+\\-=\\[\\]{};':\"\\\\|,.<>\\/?]){12,100}",
        ErrorMessage = "Password must be at least 12 characters long and contain letters, numbers, and special characters.")
    ]
    public string NewPassword { get; set; }
}