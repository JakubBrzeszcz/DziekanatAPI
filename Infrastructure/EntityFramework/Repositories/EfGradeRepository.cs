using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.EntityFramework.Repositories;

public class EfGradeRepository : EfGenericRepository<Grade>, IGradeRepository
{
    private readonly UniversityDbContext _context;
    public EfGradeRepository(UniversityDbContext context) : base(context.Grades)
    {
        _context = context;
    }

    public async Task<IEnumerable<Grade>> FindByStudentAsync(Guid studentId)
    {
        return await _context.Grades
            .Where(g => g.Student.Id == studentId)
            .Include(g => g.Course)
            .Include(g => g.Lecturer)
            .AsNoTracking()
            .ToListAsync();
    }

    public Task<IEnumerable<Grade>> FindByCourseAsync(Guid courseId)
    {
        throw new NotImplementedException();
    }
}