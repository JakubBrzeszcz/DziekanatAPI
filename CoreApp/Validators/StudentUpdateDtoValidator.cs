using CoreApp.Dto;
using FluentValidation;

namespace CoreApp.Validators;

public class StudentUpdateDtoValidator : AbstractValidator<StudentUpdateDto>
{
    public StudentUpdateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imie jest wymagane.")
            .MaximumLength(100).WithMessage("Imie nie moze przekraczac 100 znakow.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Imie zawiera niedozwolone znaki.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie moze przekraczac 200 znakow.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwisko zawiera niedozwolone znaki.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email jest wymagany.")
            .EmailAddress().WithMessage("Nieprawidlowy format adresu email.")
            .MaximumLength(200).WithMessage("Email nie moze przekraczac 200 znakow.");

        RuleFor(x => x.YearOfStudy)
            .InclusiveBetween(1, 5)
            .WithMessage("Niepoprawny rok studiow.");

        RuleFor(x => x.ProgramCode)
            .NotEmpty().WithMessage("Kod kierunku jest wymagany.")
            .MaximumLength(20).WithMessage("Kod kierunku nie moze przekraczac 20 znakow.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Niepoprawny status studenta.");
    }
}
