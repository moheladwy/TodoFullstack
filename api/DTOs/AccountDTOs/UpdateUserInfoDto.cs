using System.ComponentModel.DataAnnotations;

namespace API.DTOs.AccountDTOs;

public class UpdateUserInfoDto
{
    [Required]
    public required string Id { get; init; }

    [Range(3, 25)]
    public string? NewFirstName { get; set; }

    [Range(3, 25)]
    public string? NewLastName { get; set; }

    [EmailAddress]
    public string? NewEmail { get; set; }

    [Range(3, 100)]
    public string? NewUserName { get; set; }

    [Phone]
    public string? NewPhoneNumber { get; set; }
}