namespace Todo.Core.DTOs.AccountDTOs;

public class UserDto
{
  /// <summary>
  ///     The id of the user.
  /// </summary>
  public string Id { get; set; }

  /// <summary>
  ///     The first name of the user.
  /// </summary>
  public string FirstName { get; set; }

  /// <summary>
  ///     The last name of the user.
  /// </summary>
  public string LastName { get; set; }

  /// <summary>
  ///     The email of the user.
  /// </summary>
  public string Email { get; set; }

  /// <summary>
  ///     The username of the user.
  /// </summary>
  public string UserName { get; set; }

  /// <summary>
  ///     The phone number of the user.
  /// </summary>
  public string? PhoneNumber { get; set; }
}
