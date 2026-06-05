using CoreApp.Entities;
using CoreApp.Enums;

namespace CoreApp.Repositories;

public interface IStudentRepository : IGenericRepositoryAsync<Student>
{
    Task<IEnumerable<Student>> FindByYearOfStudyAsync(int year);
    Task<IEnumerable<Student>> FindByProgramAsync(string programCode);
    Task ChangeStatusAsync(Guid studentId, StudentStatus status);
}