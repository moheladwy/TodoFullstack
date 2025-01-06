using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.TasksDtos;

/// <summary>
///     Data transfer object for adding a task.
/// </summary>
public class AddTaskDto
{
    /// <summary>
    ///     The name of the task.
    /// </summary>
    public required string Name { get; init; }

    /// <summary>
    ///     The description of the task.
    /// </summary>
    public string? Description { get; init; }

    // public DateTime? DueDate { get; set; }

    /// <summary>
    ///     The priority of the task.
    /// </summary>
    public int Priority { get; init; } = 3;

    /// <summary>
    ///     The status of the task.
    /// </summary>
    public required Guid ListId { get; init; }
}