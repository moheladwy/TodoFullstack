namespace Todo.Core.DTOs.ListDTOs;

/// <summary>
///   ListsDto to be used for returning lists to the client.
/// </summary>
public class ListsDto
{
  /// <summary>
  ///  Id of the list.
  /// </summary>
  public Guid Id { get; set; }

  /// <summary>
  ///   Name of the list.
  /// </summary>
  public required string Name { get; set; }

  /// <summary>
  ///   Description of the list.
  /// </summary>
  public string? Description { get; set; } = string.Empty;
}
