using System.ComponentModel.DataAnnotations;

namespace API.DTOs.TasksDTOs;

public class UpdateTaskDto
{
    [Required]
    public required Guid Id { get; set; }

    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public string? Name { get; set; }

    [StringLength(200, ErrorMessage = "The field Description must not exceed 200 characters.")]
    public string? Description { get; set; }

    public bool? IsCompleted { get; set; }
}