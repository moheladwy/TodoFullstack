using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace API.Models;

/// <summary>
///     Represents a user in the application with a unique Id, Email, and Password.
///     A user has a first and last name, and a list of TaskLists.
///     Inherits from IdentityUser, which provides the Id, Email, and Password properties.
/// </summary>
public class User : IdentityUser
{
    /// <summary>
    ///     The first name of the user.
    ///     Required, must be between 3 and 25 characters.
    /// </summary>
    [Required]
    [Range(3, 25)]
    public string FirstName { get; set; } = string.Empty;

    /// <summary>
    ///     The last name of the user.
    ///     Required, must be between 3 and 25 characters.
    /// </summary>
    [Required]
    [Range(3, 25)]
    public string LastName { get; set; } = string.Empty;

    /// <summary>
    ///     The full name of the user.
    ///     Computed property that returns the concatenation of the first and last name.
    /// </summary>
    public string Name => $"{FirstName} {LastName}";

    /// <summary>
    ///     The list of TaskLists that the user has.
    ///     A user can have multiple TaskLists.
    ///     A TaskList can only belong to one user.
    /// </summary>
    public List<TaskList> Lists { get; set; } = [];
}