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
    public required Guid Id { get; set; }

    /// <summary>
    ///     The name of the list.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    ///     The description of the list.
    /// </summary>
    public string? Description { get; set; }
}