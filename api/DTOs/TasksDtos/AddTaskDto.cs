using System.ComponentModel.DataAnnotations;

namespace API.DTOs.TasksDTOs;

public class AddTaskDto
{
    [Required]
    [MinLength(3)]
    [MaxLength(100)]
    public required string Name { get; init; }

    [MaxLength(500)]
    public string? Description { get; init; }

    [Required]
    public Guid ListId { get; init; }
}