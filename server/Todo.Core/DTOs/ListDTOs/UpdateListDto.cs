using System.ComponentModel.DataAnnotations;

namespace Todo.Core.DTOs.ListDTOs;

/// <summary>
///     Represents the data transfer object for updating a list.
/// </summary>
public class UpdateListDto
{
    /// <summary>
    ///     The unique identifier of the list.
    /// </summary>
    [Required]
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the list.
    /// </summary>
    [StringLength(100, MinimumLength = 1, ErrorMessage = "The field Name must be between 1 and 100 characters.")]
    public string? Name { get; set; }

    /// <summary>
    ///     The description of the list.
    /// </summary>
    [StringLength(500, ErrorMessage = "The field Description must not exceed 500 characters.")]
    public string? Description { get; set; }
}