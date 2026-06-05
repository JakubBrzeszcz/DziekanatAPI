namespace CoreApp.Exceptions;

public class GradeNotFoundException(Guid gradeId)
    : NotFoundException($"Grade with id '{gradeId}' was not found.");