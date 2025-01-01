using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.ListDTOs;

/// <summary>
///     Data transfer object for adding a list.
/// </summary>
public class AddListDto
{
    /// <summary>
    ///     The name of the list.
    /// </summary>
    [Required]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public required string Name { get; init; }

    /// <summary>
    ///     The description of the list.
    /// </summary>
    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; init; }

    /// <summary>
    ///     The user id of the list.
    /// </summary>
    public string? UserId { get; init; }
}