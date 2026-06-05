using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface IGradeRepository : IGenericRepositoryAsync<Grade>
{
    Task<IEnumerable<Grade>> FindByStudentAsync(Guid studentId);
    Task<IEnumerable<Grade>> FindByCourseAsync(Guid courseId);
}
