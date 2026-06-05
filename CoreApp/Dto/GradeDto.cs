using CoreApp.Enums;

namespace CoreApp.Dto;

public record GradeCreateDto
{
    public Guid CourseId { get; init; }
    public Guid LecturerId { get; init; }
    public double GradeValue { get; init; }
    public DateTime DateOfIssue { get; init; }
}

public record GradeUpdateDto
{
    public double GradeValue { get; init; }
    public DateTime DateOfIssue { get; init; }
    public GradeType Type { get; init; }
}

public record GradeDto
{
    public Guid Id { get; init; }
    public string CourseName { get; init; } = string.Empty;
    public string LecturerName { get; init; } = string.Empty;
    public double Value { get; init; }
    public GradeType Type { get; init; }
    public string GradeDisplayValue { get; init; } = string.Empty;
    public DateTime DateOfIssue { get; init; }
}