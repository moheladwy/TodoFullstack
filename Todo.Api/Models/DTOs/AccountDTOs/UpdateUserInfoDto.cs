namespace Todo.Api.Models.DTOs.AccountDTOs;

using System.ComponentModel.DataAnnotations;

public class UpdateUserInfoDto
{
    [Required]
    public required string Id { get; init; }

    [StringLength(25, MinimumLength = 3)]
    public string? NewFirstName { get; set; }

    [StringLength(25, MinimumLength = 3)]
    public string? NewLastName { get; set; }

    [EmailAddress]
    public string? NewEmail { get; set; }

    [StringLength(25, MinimumLength = 3)]
    public string? NewUserName { get; set; }

    [Phone]
    public string? NewPhoneNumber { get; set; }
}