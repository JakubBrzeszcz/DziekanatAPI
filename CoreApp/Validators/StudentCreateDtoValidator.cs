using CoreApp.Dto;
using FluentValidation;

namespace CoreApp.Validators;

public class StudentCreateDtoValidator : AbstractValidator<StudentCreateDto>
{
    public StudentCreateDtoValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("Imie jest wymagane.")
            .MaximumLength(100).WithMessage("Imie nie moze przekraczac 100 znakow.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Imie zawiera niedozwolone znaki.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("Nazwisko jest wymagane.")
            .MaximumLength(200).WithMessage("Nazwisko nie moze przekraczac 200 znakow.")
            .Matches(@"^[\p{L}\s\-]+$").WithMessage("Nazwisko zawiera niedozwolone znaki.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("Numer PESEL jest wymagany.")
            .Length(11).WithMessage("Numer PESEL musi miec 11 znakow.")
            .Matches(@"^\d{11}$").WithMessage("Numer PESEL moze zawierac tylko cyfry.");

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

        RuleFor(x => x.EnrollmentYearFrom)
            .InclusiveBetween(2000, DateTime.Now.Year + 1)
            .WithMessage("Niepoprawny rok rozpoczecia studiow.");
    }
}
