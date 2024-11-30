using Todo.Api.Models.Enums;

namespace Todo.Api.Models.Entities;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

/// <summary>
///     The entity that represents a task in the database.
/// </summary>
public class Task
{
    /// <summary>
    ///     The unique identifier for the task entity, the primary key,
    ///     required, auto-generated, and a GUID (Globally Unique Identifier).
    /// </summary>
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    /// <summary>
    ///     The name of the task, required, and a string with a maximum length of 100 characters.
    ///     The minimum length is 1 character.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public required string Name { get; set; }

    /// <summary>
    ///     The description of the task, a string with a maximum length of 500 characters.
    ///     The description is optional and can be null.
    /// </summary>
    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    ///     The flag that indicates if the task is completed or not.
    ///     The default value is false, and it is required.
    /// </summary>
    [Required]
    public bool IsCompleted { get; set; }


    /// <summary>
    ///    The due date of the task, optional, and can be null.
    /// </summary>
    [DataType(DataType.Date)]
    public DateTime? DueDate { get; set; }

    /// <summary>
    ///     The priority of the task, optional, and can be null.
    /// </summary>
    [DataType(DataType.Custom)]
    public TaskPriority? Priority { get; set; } = TaskPriority.Low;

    /// <summary>
    ///     The unique identifier of the task list that the task belongs to.
    ///     The foreign key is required and can be null.
    /// </summary>
    [ForeignKey("ListId")]
    public Guid? ListId { get; set; }

    /// <summary>
    ///     The navigation property to the task list that the task belongs to.
    ///     The task list is optional and can be null.
    ///     The property is not serialized in the JSON response.
    ///     The property is not mapped to the database.
    /// </summary>
    [JsonIgnore]
    public virtual TaskList? TaskList { get; set; }
}