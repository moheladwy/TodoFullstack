using System.ComponentModel.DataAnnotations;
using Todo.Core.Enums;

namespace Todo.Core.DTOs.TasksDtos;

/// <summary>
///     Represents the data transfer object for updating a task.
/// </summary>
public class UpdateTaskDto
{
    /// <summary>
    ///     Gets or sets the task identifier.
    /// </summary>
    [Required]
    public required Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the task name.
    /// </summary>
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public string? Name { get; set; }

    /// <summary>
    ///     Gets or sets the task description.
    /// </summary>
    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; set; }

    /// <summary>
    ///     Gets or sets the task due date.
    /// </summary>
    // [DataType(DataType.Date)]
    // public DateTime? DueDate { get; set; }

    /// <summary>
    ///     Gets or sets the task priority.
    /// </summary>
    public TaskPriority Priority { get; set; } = TaskPriority.Low;

    /// <summary>
    ///     Gets or sets the task status.
    /// </summary>
    public bool? IsCompleted { get; set; }
}