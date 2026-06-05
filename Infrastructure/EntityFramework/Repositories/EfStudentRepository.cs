using CoreApp.Entities;
using CoreApp.Repositories;
using CoreApp.Enums;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework.Repositories;

public class EfStudentRepository : EfGenericRepository<Student>, IStudentRepository
{
    private readonly UniversityDbContext _context;

    public EfStudentRepository(UniversityDbContext context) : base(context.Students)
    {
        _context = context;
    }

    public Task<Student?> FindByEmailAsync(string email)
    {
        return _context.Students.FirstOrDefaultAsync(s => s.Email == email);
    }

    // Nadpisujemy metodę z repozytorium generycznego, aby dociągnąć powiązane dane.
    // Jest to kluczowa zaleta posiadania specyficznych repozytoriów.
    public override async Task<Student?> FindByIdAsync(Guid id)
    {
        return await _context.Students
            .Include(s => s.DegreeProgram)
            .Include(s => s.EnrollmentYear)
            .Include(s => s.Grades)
                .ThenInclude(g => g.Course)
            .Include(s => s.Grades)
                .ThenInclude(g => g.Lecturer)
            .FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task ChangeStatusAsync(Guid id, StudentStatus status)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is not null)
        {
            student.Status = status;
            // Zapis zmian nastąpi w UnitOfWork
        }
    }

    public async Task<IEnumerable<Student>> FindByYearOfStudyAsync(int year)
    {
        return await _context.Students
            .Where(s => s.YearOfStudy == year).ToListAsync();
    }

    public async Task<IEnumerable<Student>> FindByProgramAsync(string programCode)
    {
        return await _context.Students
            .Where(s => s.DegreeProgram != null && s.DegreeProgram.Code == programCode).ToListAsync();
    }
}