using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models;

public class TaskList
{
    [Key]
    public string Id { get; init; }

    [Required]
    [Range(1, 100)]
    public required string Name { get; set; }

    [Range(0, 200)]
    public string Description { get; set; }

    [Required] public List<Task> Tasks { get; set; } = [];

    [ForeignKey("UserId")]
    public User? User { get; set; }
}