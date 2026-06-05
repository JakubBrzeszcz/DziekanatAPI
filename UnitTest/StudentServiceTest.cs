using AutoMapper;
using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Services;
using CoreApp.UnitOfWork;
using CoreApp.Module;
using CoreApp.Exceptions;
using Xunit;

namespace UnitTest;

public class StudentServiceTest
{
    private readonly IStudentService _studentService;
    private readonly IUniversityUnitOfWork _unitOfWork;

    public StudentServiceTest()
    {
        // Tworzymy kompletne, izolowane środowisko dla naszego serwisu.
        _unitOfWork = new TestUniversityUnitOfWork();
        
        // Konfigurujemy prawdziwy AutoMapper z naszym profilem
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<StudentsMappingProfile>();
        });
        var mapper = mapperConfig.CreateMapper();
        _studentService = new StudentService(_unitOfWork, mapper);
    }

    [Fact]
    public async Task AddStudent_ViaService_ShouldCreateAndReturnStudent()
    {
        // ARRANGE: Przygotowujemy dane wejściowe (DTO)
        var dto = new StudentCreateDto { FirstName = "Anna", LastName = "Testowa", NationalId = "95050512345", Email = "anna.testowa@example.com", YearOfStudy = 2, ProgramCode = "IT-BSC", EnrollmentYearFrom = 2023 };

        // ACT: Wywołujemy metodę serwisu, którą testujemy
        var createdStudentDto = await _studentService.AddStudent(dto);

        // ASSERT: Sprawdzamy, czy wynik jest zgodny z oczekiwaniami
        Assert.NotNull(createdStudentDto);
        Assert.Equal("Anna", createdStudentDto.FirstName);

        // Dodatkowa weryfikacja: czy student faktycznie został zapisany?
        var foundStudentDto = await _studentService.FindStudentByIdAsync(createdStudentDto.Id);
        Assert.NotNull(foundStudentDto);
        Assert.Equal(createdStudentDto.Id, foundStudentDto.Id);
    }

    [Fact]
    public async Task UpdateStudent_WhenExists_ShouldUpdateAndReturnDto()
    {
        // ARRANGE: Stwórz studenta, którego będziemy aktualizować
        var createDto = new StudentCreateDto { FirstName = "Jan", LastName = "Kowalski", NationalId = "90010112345", Email = "jan.kowalski@example.com", YearOfStudy = 1, ProgramCode = "CS-BSC", EnrollmentYearFrom = 2023 };
        var createdStudentDto = await _studentService.AddStudent(createDto);

        var updateDto = new StudentUpdateDto
        {
            Id = createdStudentDto.Id,
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski.new@example.com", // Zmieniony email
            YearOfStudy = 2, // Zmieniony rok
            ProgramCode = "CS-BSC",
            Status = CoreApp.Enums.StudentStatus.Active
        };

        // ACT: Zaktualizuj studenta
        var updatedStudentDto = await _studentService.UpdateStudentAsync(createdStudentDto.Id, updateDto);

        // ASSERT: Sprawdź, czy aktualizacja się powiodła
        Assert.NotNull(updatedStudentDto);
        Assert.Equal(createdStudentDto.Id, updatedStudentDto.Id);
        Assert.Equal(2, updatedStudentDto.YearOfStudy); // Weryfikacja zmiany
        Assert.Equal("jan.kowalski.new@example.com", updatedStudentDto.Email);

        // Dodatkowa weryfikacja przez ponowne pobranie
        var fetchedStudent = await _studentService.FindStudentByIdAsync(createdStudentDto.Id);
        Assert.NotNull(fetchedStudent);
        Assert.Equal("jan.kowalski.new@example.com", fetchedStudent.Email);
    }

    [Fact]
    public async Task DeleteStudent_WhenExists_ShouldSucceedAndRemoveStudent()
    {
        // ARRANGE: Stwórz studenta do usunięcia
        var createdStudentDto = await _studentService.AddStudent(new StudentCreateDto { FirstName = "Anna", LastName = "Nowak", NationalId = "91020212345", Email = "anna.nowak@example.com", YearOfStudy = 3, ProgramCode = "EE-MSC", EnrollmentYearFrom = 2021 });
        var wasDeleted = await _studentService.DeleteStudentAsync(createdStudentDto.Id);
        Assert.True(wasDeleted);
        Assert.Null(await _studentService.FindStudentByIdAsync(createdStudentDto.Id));
    }

    [Fact]
    public async Task AddStudent_WithValidProgramCode_ShouldAssignDegreeProgram()
    {
        // ARRANGE: Dodajmy program studiów do naszego testowego repozytorium
        var program = new DegreeProgram { Id = Guid.NewGuid(), Code = "IT-TEST", Name = "Informatyka Testowa" };
        await _unitOfWork.DegreePrograms.AddAsync(program);

        var dto = new StudentCreateDto { FirstName = "Kamil", LastName = "Programista", NationalId = "99121212345", Email = "kamil.programista@example.com", YearOfStudy = 1, ProgramCode = "IT-TEST", EnrollmentYearFrom = 2023 };

        // ACT
        var createdStudentDto = await _studentService.AddStudent(dto);

        // ASSERT
        Assert.NotNull(createdStudentDto);
        Assert.Equal("IT-TEST", createdStudentDto.ProgramCode);
        Assert.Equal("Informatyka Testowa", createdStudentDto.ProgramName);
    }

    [Fact]
    public async Task UpdateStudent_WhenNotExists_ShouldReturnNull()
    {
        // ARRANGE: Używamy losowego ID, które na pewno nie istnieje
        var nonExistentId = Guid.NewGuid();
        var updateDto = new StudentUpdateDto { Id = nonExistentId, FirstName = "Duch", LastName = "Student", Email = "ghost@example.com", YearOfStudy = 1, ProgramCode = "N/A", Status = StudentStatus.Inactive };

        // ACT: Próbujemy zaktualizować nieistniejącego studenta
        var result = await _studentService.UpdateStudentAsync(nonExistentId, updateDto);

        // ASSERT: Sprawdzamy, czy serwis poprawnie zwrócił null
        Assert.Null(result);
    }

    [Fact]
    public async Task AddGrade_WhenStudentExists_ShouldAddGrade()
    {
        // ARRANGE
        var student = await _studentService.AddStudent(new StudentCreateDto { FirstName = "Test", LastName = "Student", NationalId = "11111111111", Email = "test@test.com" });
        var course = new Course { Id = Guid.NewGuid(), Name = "Test Course" };
        await _unitOfWork.Courses.AddAsync(course);
        var lecturer = new Lecturer { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Lecturer" };
        await _unitOfWork.Lecturers.AddAsync(lecturer);

        var gradeDto = new GradeCreateDto { CourseId = course.Id, LecturerId = lecturer.Id, GradeValue = 5.0, DateOfIssue = DateTime.Now };

        // ACT
        var result = await _studentService.AddGradeAsync(student.Id, gradeDto);

        // ASSERT
        Assert.NotNull(result);
        Assert.Equal(5.0, result.Value);
        Assert.Equal(course.Name, result.CourseName);

        var studentDetails = await _studentService.FindStudentByIdAsync(student.Id);
        Assert.NotNull(studentDetails);
        Assert.Single(studentDetails.Grades);
    }

    [Fact]
    public async Task AddGrade_WhenLecturerDoesNotExist_ShouldThrowLecturerNotFoundException()
    {
        // ARRANGE
        var student = await _studentService.AddStudent(new StudentCreateDto { FirstName = "Test", LastName = "Student", NationalId = "22222222222", Email = "test2@test.com" });
        var course = new Course { Id = Guid.NewGuid(), Name = "Test Course" };
        await _unitOfWork.Courses.AddAsync(course);
        var nonExistentLecturerId = Guid.NewGuid();
        var gradeDto = new GradeCreateDto { CourseId = course.Id, LecturerId = nonExistentLecturerId, GradeValue = 4.0, DateOfIssue = DateTime.Now };

        // ACT & ASSERT
        await Assert.ThrowsAsync<LecturerNotFoundException>(() => _studentService.AddGradeAsync(student.Id, gradeDto));
    }

    [Fact]
    public async Task UpdateGrade_WhenGradeExists_ShouldUpdateAndReturnDto()
    {
        // ARRANGE: Stwórz studenta i dodaj mu ocenę
        var student = await _studentService.AddStudent(new StudentCreateDto { FirstName = "Test", LastName = "Student", NationalId = "33333333333", Email = "test3@test.com" });
        var course = new Course { Id = Guid.NewGuid(), Name = "Test Course" };
        await _unitOfWork.Courses.AddAsync(course);
        var lecturer = new Lecturer { Id = Guid.NewGuid(), FirstName = "Test", LastName = "Lecturer" };
        await _unitOfWork.Lecturers.AddAsync(lecturer);
        var createGradeDto = new GradeCreateDto { CourseId = course.Id, LecturerId = lecturer.Id, GradeValue = 3.0, DateOfIssue = DateTime.Now.AddDays(-10) };
        var createdGrade = await _studentService.AddGradeAsync(student.Id, createGradeDto);

        var updateDto = new GradeUpdateDto
        {
            GradeValue = 4.5,
            DateOfIssue = DateTime.Now.AddDays(-1),
            Type = GradeType.Final
        };

        // ACT: Zaktualizuj ocenę
        var updatedGrade = await _studentService.UpdateGradeAsync(student.Id, createdGrade.Id, updateDto);

        // ASSERT: Sprawdź, czy aktualizacja się powiodła
        Assert.NotNull(updatedGrade);
        Assert.Equal(4.5, updatedGrade.Value);
        Assert.Equal(GradeType.Final, updatedGrade.Type);
    }
}