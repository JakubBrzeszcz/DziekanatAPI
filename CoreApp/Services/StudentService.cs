using AutoMapper;
using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Pagination;
using CoreApp.Enums;
using CoreApp.Repositories;
using CoreApp.Exceptions;
using CoreApp.Services;
using CoreApp.UnitOfWork;

namespace CoreApp.Services;

public class StudentService(IUniversityUnitOfWork unitOfWork, IMapper mapper) : IStudentService
{
    public async Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size)
    {
        var pagedResult = await unitOfWork.Students.FindPagedAsync(page, size);
        var dtos = mapper.Map<List<StudentSummaryDto>>(pagedResult.Items);
        return new PagedResult<StudentSummaryDto>(dtos, pagedResult.TotalCount, page, size);
    }

    public async Task<StudentDetailDto?> FindStudentByIdAsync(Guid id)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student is null)
            return null;

        return mapper.Map<StudentDetailDto>(student);
    }

    public async Task<StudentDetailDto> AddStudent(StudentCreateDto dto)
    {
        // Używamy AutoMappera do stworzenia encji z DTO
        var student = mapper.Map<Student>(dto);

        // Logika biznesowa, która nie należy do mappera
        var program = await unitOfWork.DegreePrograms.FindByCodeAsync(dto.ProgramCode);
        if (program is not null)
        {
            student.DegreeProgram = program;
        }

        await unitOfWork.Students.AddAsync(student);
        await unitOfWork.SaveChangesAsync();

        // Zwracamy zmapowane DTO, a nie encję
        return mapper.Map<StudentDetailDto>(student);
    }

    public async Task<StudentSummaryDto?> UpdateStudentAsync(Guid id, StudentUpdateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(id);
        if (student is null)
            return null;

        // Używamy AutoMappera do nałożenia zmian z DTO na istniejącą encję
        mapper.Map(dto, student);

        // Logika biznesowa
        if (student.DegreeProgram?.Code != dto.ProgramCode)
        {
            var program = await unitOfWork.DegreePrograms.FindByCodeAsync(dto.ProgramCode);
            student.DegreeProgram = program;
            student.ProgramName = program?.Name ?? dto.ProgramCode;
        }

        await unitOfWork.Students.UpdateAsync(student);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<StudentSummaryDto>(student);
    }

    public async Task<bool> DeleteStudentAsync(Guid id)
    {
        try
        {
            await unitOfWork.Students.RemoveByIdAsync(id);
            await unitOfWork.SaveChangesAsync();
            return true;
        }
        catch (KeyNotFoundException)
        {
            return false;
        }
    }

    public async Task ChangeStudentStatusAsync(Guid studentId, StudentStatus status)
    {
        await unitOfWork.Students.ChangeStatusAsync(studentId, status);
        await unitOfWork.SaveChangesAsync();
    }

    public async Task<GradeDto> AddGradeAsync(Guid studentId, GradeCreateDto dto)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId);
        if (student is null)
            throw new StudentNotFoundException(studentId);

        var course = await unitOfWork.Courses.FindByIdAsync(dto.CourseId);
        if (course is null)
            throw new CourseNotFoundException(dto.CourseId);

        var lecturer = await unitOfWork.Lecturers.FindByIdAsync(dto.LecturerId);
        if (lecturer is null)
            throw new LecturerNotFoundException(dto.LecturerId);

        var grade = new Grade
        {
            Student = student,
            Course = course,
            Lecturer = lecturer,
            GradeValue = GradeValueExtensions.FromDouble(dto.GradeValue),
            DateOfIssue = dto.DateOfIssue
        };

        student.Grades.Add(grade);
        await unitOfWork.Grades.AddAsync(grade);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<GradeDto>(grade);
    }

    public async Task<IEnumerable<GradeDto>> GetGradesAsync(Guid studentId)
    {
        var student = await unitOfWork.Students.FindByIdAsync(studentId);
        if (student is null)
            throw new StudentNotFoundException(studentId);

        var grades = await unitOfWork.Grades.FindByStudentAsync(studentId);

        return mapper.Map<IEnumerable<GradeDto>>(grades);
    }

    public async Task<GradeDto?> UpdateGradeAsync(Guid studentId, Guid gradeId, GradeUpdateDto dto)
    {
        var grade = await unitOfWork.Grades.FindByIdAsync(gradeId);

        // Sprawdzamy, czy ocena istnieje i czy należy do właściwego studenta
        if (grade is null || grade.Student.Id != studentId)
        {
            throw new GradeNotFoundException(gradeId);
        }

        // Używamy AutoMappera do nałożenia zmian z DTO na istniejącą encję
        mapper.Map(dto, grade);

        await unitOfWork.Grades.UpdateAsync(grade);
        await unitOfWork.SaveChangesAsync();

        return mapper.Map<GradeDto>(grade);
    }
}