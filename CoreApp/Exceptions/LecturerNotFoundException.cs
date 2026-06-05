namespace CoreApp.Exceptions;

public class LecturerNotFoundException(Guid lecturerId)
    : NotFoundException($"Lecturer with id '{lecturerId}' was not found.");