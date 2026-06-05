using CoreApp.Dto;
using FluentValidation;

namespace CoreApp.Validators;

public class GradeUpdateDtoValidator : AbstractValidator<GradeUpdateDto>
{
    private static readonly double[] ValidGrades = [2.0, 3.0, 3.5, 4.0, 4.5, 5.0];

    public GradeUpdateDtoValidator()
    {
        RuleFor(x => x.GradeValue)
            .Must(BeAValidGrade).WithMessage("Nieprawidłowa wartość oceny. Dopuszczalne wartości to 2.0, 3.0, 3.5, 4.0, 4.5, 5.0.");

        RuleFor(x => x.DateOfIssue)
            .NotEmpty().WithMessage("Data wystawienia oceny jest wymagana.")
            .LessThanOrEqualTo(DateTime.Now).WithMessage("Data wystawienia nie może być z przyszłości.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Nieprawidłowy typ oceny.");
    }

    private bool BeAValidGrade(double grade)
    {
        return ValidGrades.Contains(grade);
    }
}