namespace API.Exceptions;

/// <summary>
///     Exception thrown when a role is not added to the database.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class AddRoleException(string message) : Exception(message);

/// <summary>
///     Exception thrown when a user is not added to the database.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class CreateUserException(string message) : Exception(message);

/// <summary>
///     Exception thrown when the email is not valid.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class InvalidEmailException(string message) : Exception(message);

/// <summary>
///     Exception thrown when the id guid is not valid.
/// </summary>
/// <param name="invalidGuidFormat">
///     The message to display when the exception is thrown.
/// </param>
public class InvalidGuidException(string invalidGuidFormat) : Exception(invalidGuidFormat);

/// <summary>
///     Exception thrown when the password is not valid.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class InvalidPasswordException(string message) : Exception(message);

/// <summary>
///     Exception thrown when the list is not found in the database.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class ListNotFoundException(string message) : Exception(message);

/// <summary>
///     Exception thrown when the password is did not change.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class PasswordDidNotChangeException(string message) : Exception(message);

/// <summary>
///     Exception thrown when the task is not found in the database.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class TaskNotFoundException(string message) : Exception(message);

/// <summary>
///     Exception thrown when the user information is not updated in the database.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class UserInformationDidNotUpdateException(string message) : Exception(message);

/// <summary>
///     Exception thrown when the user is not found in the database.
/// </summary>
/// <param name="message">
///     The message to display when the exception is thrown.
/// </param>
public class UserNotFoundException(string message) : Exception(message);