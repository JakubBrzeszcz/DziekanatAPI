using CoreApp.Dto;
using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Pagination;

namespace CoreApp.Services;

public interface IStudentService
{
    Task<PagedResult<StudentSummaryDto>> FindAllStudentsPaged(int page, int size);
    Task<StudentDetailDto?> FindStudentByIdAsync(Guid id);
    Task<StudentDetailDto> AddStudent(StudentCreateDto dto);    
    Task<StudentSummaryDto?> UpdateStudentAsync(Guid id, StudentUpdateDto dto);
    Task<bool> DeleteStudentAsync(Guid id);
    Task ChangeStudentStatusAsync(Guid studentId, StudentStatus status);
    Task<GradeDto> AddGradeAsync(Guid studentId, GradeCreateDto dto);
    Task<IEnumerable<GradeDto>> GetGradesAsync(Guid studentId);
    Task<GradeDto?> UpdateGradeAsync(Guid studentId, Guid gradeId, GradeUpdateDto dto);
}