using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public class Task
{
    [Key]
    public required string Id { get; init; }

    [Required]
    [Range(1, 100)]
    public required string Name { get; set; }

    [Range(0, 200)]
    public string? Description { get; set; } = string.Empty;

    [Required]
    public required bool IsComplete { get; set; }

    [ForeignKey("ListId")]
    public string? ListId { get; set; }
}