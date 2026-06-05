namespace CoreApp.Exceptions;

public class CourseNotFoundException(Guid courseId)
    : NotFoundException($"Course with id '{courseId}' was not found.");