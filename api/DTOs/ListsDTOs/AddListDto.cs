using System.ComponentModel.DataAnnotations;

namespace API.DTOs.ListsDTOs;

public class AddListDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string Name { get; init; }

    [MaxLength(500)]
    public string? Description { get; init; }

    // [Required]
    // public string UserId { get; set; }
}