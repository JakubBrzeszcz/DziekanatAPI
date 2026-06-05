using AutoMapper;
using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using CoreApp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CoreApp.Module;

public class StudentsMappingProfile : Profile
{
    public StudentsMappingProfile()
    {
        // Mapowanie z encji na DTO
        CreateMap<Student, StudentSummaryDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.DegreeProgram != null ? src.DegreeProgram.Name : src.ProgramName))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.NationalId));

        CreateMap<Student, StudentDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.StudentId, opt => opt.MapFrom(src => src.NationalId))
            .ForMember(dest => dest.ProgramCode, opt => opt.MapFrom(src => src.DegreeProgram != null ? src.DegreeProgram.Code : string.Empty))
            .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.DegreeProgram != null ? src.DegreeProgram.Name : src.ProgramName))
            .ForMember(dest => dest.EnrollmentYear, opt => opt.MapFrom(src => src.EnrollmentYear != null ? src.EnrollmentYear.Name : string.Empty))
            .ForMember(dest => dest.TotalEctsEarned, opt => opt.MapFrom(src => src.Grades != null ? src.Grades.Sum(g => g.Course != null ? g.Course.EctsCredits : 0) : 0))
            .ForMember(dest => dest.GradePointAverage, opt => opt.MapFrom(src => src.Grades != null && src.Grades.Any() ? Math.Round(src.Grades.Average(g => g.GradeValue.Value()), 2) : 0.0))
            .ForMember(dest => dest.IsEligibleForDiploma, opt => opt.MapFrom(src => src.DegreeProgram != null && src.Grades != null && src.Grades.Sum(g => g.Course != null ? g.Course.EctsCredits : 0) >= src.DegreeProgram.MinEctsForDiploma))
            .ForMember(dest => dest.Grades, opt => opt.MapFrom(src => src.Grades));

        // Mapowanie z DTO na encję
        CreateMap<StudentCreateDto, Student>()
            .ForMember(dest => dest.ProgramName, opt => opt.MapFrom(src => src.ProgramCode))
            .ForMember(dest => dest.EnrollmentYearValue, opt => opt.MapFrom(src => src.EnrollmentYearFrom))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => StudentStatus.Active))
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            // Jawnie ignorujemy właściwości nawigacyjne, które są ustawiane w logice biznesowej serwisu
            .ForMember(dest => dest.DegreeProgram, opt => opt.Ignore())
            .ForMember(dest => dest.EnrollmentYear, opt => opt.Ignore())
            .ForMember(dest => dest.Grades, opt => opt.Ignore());

        CreateMap<StudentUpdateDto, Student>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.NationalId, opt => opt.Ignore());

        // Mapowanie ocen
        CreateMap<Grade, GradeDto>()
            .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.GradeValue.Value()))
            .ForMember(dest => dest.GradeDisplayValue, opt => opt.MapFrom(src => src.GradeValue.ToDisplayString()))
            .ForMember(dest => dest.CourseName, opt => opt.MapFrom(src => src.Course.Name))
            .ForMember(dest => dest.LecturerName, opt => opt.MapFrom(src => $"{src.Lecturer.Title} {src.Lecturer.FirstName} {src.Lecturer.LastName}"));

        // Mapowanie do aktualizacji oceny
        CreateMap<GradeUpdateDto, Grade>()
            .ForMember(dest => dest.GradeValue, opt => opt.MapFrom(src => GradeValueExtensions.FromDouble(src.GradeValue)));
    }
}

public static class StudentsModule
{
    public static IServiceCollection AddStudentsModule(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // Rejestracja wszystkich walidatorów z tego assembly (CoreApp)
        services.AddValidatorsFromAssembly(typeof(StudentsModule).Assembly);
        
        // Włączenie automatycznej walidacji dla wszystkich kontrolerów ASP.NET Core
        services.AddFluentValidationAutoValidation();

        // Rejestracja AutoMappera i profili z tego samego assembly co StudentsMappingProfile
        services.AddAutoMapper(typeof(StudentsModule).Assembly);

        // Rejestracja serwisów z warstwy Core
        services.AddScoped<IStudentService, StudentService>();

        return services;
    }
}