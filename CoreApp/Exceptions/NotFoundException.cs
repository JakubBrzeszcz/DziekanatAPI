namespace CoreApp.Exceptions;

public abstract class NotFoundException(string message) : Exception(message);