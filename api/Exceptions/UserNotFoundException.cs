namespace API.Exceptions;

public class UserNotFoundException(string message) : Exception(message);