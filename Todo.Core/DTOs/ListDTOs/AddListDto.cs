using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.ListDTOs;

public class AddListDto
{
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public required string Name { get; init; }

    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; init; }

    [Required]
    public required string UserId { get; init; }
}