using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.EntityFramework.Repositories;

public class EfLecturerRepository : EfGenericRepository<Lecturer>, ILecturerRepository
{
    private readonly UniversityDbContext _context;
    public EfLecturerRepository(UniversityDbContext context) : base(context.Lecturers)
    {
        _context = context;
    }

    public Task<IEnumerable<Lecturer>> FindByCourse(Guid courseId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Lecturer>> FindByFacultyAsync(string faculty)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Lecturer>> FindByTitle(string title)
    {
        throw new NotImplementedException();
    }
}