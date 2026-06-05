using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.EntityFramework.Repositories;

public class EfDegreeProgramRepository : EfGenericRepository<DegreeProgram>, IDegreeProgramRepository
{
    private readonly UniversityDbContext _context;
    public EfDegreeProgramRepository(UniversityDbContext context) : base(context.DegreePrograms)
    {
        _context = context;
    }

    public Task<DegreeProgram?> FindByCodeAsync(string code)
    {
        return _context.DegreePrograms.FirstOrDefaultAsync(p => p.Code == code);
    }

    public Task<IEnumerable<DegreeProgram>> FindByFacultyAsync(string faculty)
    {
        throw new System.NotImplementedException();
    }
}