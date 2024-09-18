using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

public class User : IdentityUser
{
    [Required]
    [Range(3, 25)]
    public string FirstName { get; set; } = string.Empty;

    [Required]
    [Range(3, 25)]
    public string LastName { get; set; } = string.Empty;

    public string Name => $"{FirstName} {LastName}";

    public List<TaskList> Lists { get; set; } = [];
}