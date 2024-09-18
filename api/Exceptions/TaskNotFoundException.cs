namespace API.Exceptions;

public class TaskNotFoundException(string message) : Exception(message) { }