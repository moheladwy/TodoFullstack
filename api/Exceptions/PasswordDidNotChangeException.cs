namespace API.Exceptions;

public class PasswordDidNotChangeException(string message) : Exception(message) { }