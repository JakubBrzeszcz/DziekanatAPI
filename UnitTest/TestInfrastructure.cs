using CoreApp.Entities;
using CoreApp.Repositories;
using CoreApp.UnitOfWork;
using Infrastructure.Memory;

namespace UnitTest;

// Prosta implementacja IStudentRepository na potrzeby testów,
// dziedzicząca po istniejącej, generycznej implementacji.
public class MemoryStudentRepository : MemoryGenericRepository<Student>, IStudentRepository
{
    public async Task ChangeStatusAsync(Guid id, CoreApp.Enums.StudentStatus status)
    {
        var student = await FindByIdAsync(id);
        if (student is not null)
        {
            student.Status = status;
            await UpdateAsync(student);
        }
    }

    public Task<IEnumerable<Student>> FindByYearOfStudyAsync(int year)
    {
        return Task.FromResult(_data.Values.Where(s => s.YearOfStudy == year).AsEnumerable());
    }

    public Task<IEnumerable<Student>> FindByProgramAsync(string programCode)
    {
        return Task.FromResult(_data.Values.Where(s => s.ProgramName.Equals(programCode, StringComparison.OrdinalIgnoreCase)).AsEnumerable());
    }
}

// Prosta implementacja Unit of Work na potrzeby testów.
public class TestUniversityUnitOfWork : IUniversityUnitOfWork
{
    public IStudentRepository Students { get; } = new MemoryStudentRepository();
    public ILecturerRepository Lecturers { get; } = new InMemoryLecturerRepository();
    public IGradeRepository Grades { get; } = new InMemoryGradeRepository();
    public ICourseRepository Courses { get; } = new InMemoryCourseRepository();
    public IDegreeProgramRepository DegreePrograms { get; } = new InMemoryDegreeProgramRepository();

    public Task<int> SaveChangesAsync()
    {
        return Task.FromResult(0);
    }

    public Task BeginTransactionAsync() => Task.CompletedTask;

    public Task CommitTransactionAsync() => Task.CompletedTask;

    public Task RollbackTransactionAsync() => Task.CompletedTask;

    public ValueTask DisposeAsync() => ValueTask.CompletedTask;
}