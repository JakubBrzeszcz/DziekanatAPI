using CoreApp.Dto;
using FluentValidation;

namespace CoreApp.Validators;

public class GradeCreateDtoValidator : AbstractValidator<GradeCreateDto>
{
    private static readonly double[] ValidGrades = [2.0, 3.0, 3.5, 4.0, 4.5, 5.0];

    public GradeCreateDtoValidator()
    {
        RuleFor(x => x.CourseId)
            .NotEmpty().WithMessage("Identyfikator kursu jest wymagany.");

        RuleFor(x => x.LecturerId)
            .NotEmpty().WithMessage("Identyfikator wykładowcy jest wymagany.");

        RuleFor(x => x.GradeValue)
            .Must(BeAValidGrade).WithMessage("Nieprawidłowa wartość oceny. Dopuszczalne wartości to 2.0, 3.0, 3.5, 4.0, 4.5, 5.0.");

        RuleFor(x => x.DateOfIssue)
            .NotEmpty().WithMessage("Data wystawienia oceny jest wymagana.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Data wystawienia nie może być z przyszłości.");
    }

    private bool BeAValidGrade(double grade)
    {
        return ValidGrades.Contains(grade);
    }
}