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
    public required Guid Id { get; set; }

    /// <summary>
    ///     Gets or sets the task name.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     Gets or sets the task description.
    /// </summary>
    public string? Description { get; set; }

    // public DateTime? DueDate { get; set; }

    /// <summary>
    ///     Gets or sets the task priority.
    /// </summary>
    public TaskPriority Priority { get; set; } = TaskPriority.Low;

    /// <summary>
    ///     Gets or sets the task status.
    /// </summary>
    public bool IsCompleted { get; set; }
}