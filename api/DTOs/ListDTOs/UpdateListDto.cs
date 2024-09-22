using System.ComponentModel.DataAnnotations;

namespace api.DTOs.ListDTOs;

public class UpdateListDto
{
    [Required] public required Guid Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public string Name { get; set; }

    [StringLength(200, ErrorMessage = "The field Description must not exceed 200 characters.")]
    public string? Description { get; set; }
}