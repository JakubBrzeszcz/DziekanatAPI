using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.EntityFramework.Repositories;

public class EfCourseRepository : EfGenericRepository<Course>, ICourseRepository
{
    private readonly UniversityDbContext _context;
    public EfCourseRepository(UniversityDbContext context) : base(context.Courses)
    {
        _context = context;
    }

    public Task<IEnumerable<Course>> FindByDegreeProgramAsync(string programCode)
    {
        throw new NotImplementedException();
    }
}