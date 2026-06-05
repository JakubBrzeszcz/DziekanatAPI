using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class InMemoryGradeRepository : MemoryGenericRepository<Grade>, IGradeRepository
{
    public Task<IEnumerable<Grade>> FindByStudentAsync(Guid studentId)
    {
        var result = _data.Values
            .Where(g => g.Student != null && g.Student.Id == studentId)
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Grade>> FindByCourseAsync(Guid courseId)
    {
        var result = _data.Values
            .Where(g => g.Course != null && g.Course.Id == courseId)
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }
}