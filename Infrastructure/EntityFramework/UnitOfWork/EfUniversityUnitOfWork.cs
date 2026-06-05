using CoreApp.Repositories;
using CoreApp.UnitOfWork;
using Infrastructure.EntityFramework.Context;
using System.Threading.Tasks;

namespace Infrastructure.EntityFramework.UnitOfWork;

public class EfUniversityUnitOfWork(
    UniversityDbContext context,
    IStudentRepository studentRepository,
    IDegreeProgramRepository degreeProgramRepository,
    ILecturerRepository lecturerRepository,
    ICourseRepository courseRepository,
    IGradeRepository gradeRepository
    ) : IUniversityUnitOfWork
{
    public IStudentRepository Students => studentRepository;
    public IDegreeProgramRepository DegreePrograms => degreeProgramRepository;
    public ILecturerRepository Lecturers => lecturerRepository;
    public ICourseRepository Courses => courseRepository;
    public IGradeRepository Grades => gradeRepository;

    public Task<int> SaveChangesAsync()
    {
        return context.SaveChangesAsync();
    }

    public Task BeginTransactionAsync()
    {
        return context.Database.BeginTransactionAsync();
    }

    public Task CommitTransactionAsync()
    {
        return context.Database.CommitTransactionAsync();
    }

    public Task RollbackTransactionAsync()
    {
        return context.Database.RollbackTransactionAsync();
    }
    public ValueTask DisposeAsync()
    {
        return context.DisposeAsync();
    }
}