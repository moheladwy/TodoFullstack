using System.ComponentModel.DataAnnotations;
using Todo.Core.Enums;

namespace Todo.Core.DTOs.TasksDtos;

/// <summary>
///     Data transfer object for adding a task.
/// </summary>
public class AddTaskDto
{
    /// <summary>
    ///     The name of the task.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public required string Name { get; init; }

    /// <summary>
    ///     The description of the task.
    /// </summary>
    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; init; }

    /// <summary>
    ///     The due date of the task.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    /// <summary>
    ///     The priority of the task.
    /// </summary>
    [DataType(DataType.Custom)]
    public TaskPriority? Priority { get; init; } = TaskPriority.Low;

    /// <summary>
    ///     The status of the task.
    /// </summary>
    [Required]
    public Guid ListId { get; init; }
}