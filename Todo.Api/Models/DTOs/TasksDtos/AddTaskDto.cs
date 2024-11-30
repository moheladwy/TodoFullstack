using System;

namespace Todo.Api.Models.DTOs.TasksDtos;

using Enums;
using System.ComponentModel.DataAnnotations;

public class AddTaskDto
{
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public required string Name { get; init; }

    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; init; }

    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    [DataType(DataType.Custom)]
    public TaskPriority? Priority { get; init; } = TaskPriority.Low;

    [Required]
    public Guid ListId { get; init; }
}