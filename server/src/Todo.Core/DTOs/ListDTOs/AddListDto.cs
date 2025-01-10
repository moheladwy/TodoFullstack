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
    public required string Name { get; init; }

    /// <summary>
    ///     The description of the list.
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    ///     The user id of the list.
    /// </summary>
    public string? UserId { get; init; }
}