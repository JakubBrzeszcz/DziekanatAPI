namespace CoreApp.Exceptions;

public class StudentNotFoundException(Guid studentId)
    : NotFoundException($"Student with id '{studentId}' was not found.");