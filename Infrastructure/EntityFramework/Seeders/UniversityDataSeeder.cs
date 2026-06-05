using CoreApp.Interfaces;
using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Infrastructure.EntityFramework.Context;

namespace Infrastructure.EntityFramework.Seeders;

public class UniversityDataSeeder : IDataSeeder
{
    private readonly UniversityDbContext _context;

    public UniversityDataSeeder(UniversityDbContext context)
    {
        _context = context;
    }

    public int Order => 1;

    public async Task SeedAsync()
    {
        // 1. Dodajemy testowy kurs
        var courseId = Guid.Parse("c1a2b3d4-e5f6-7890-1234-567890abcdef");
        if (!await _context.Courses.AnyAsync(c => c.Id == courseId))
        {
            _context.Courses.Add(new Course { Id = courseId, Name = "Programowanie Obiektowe", EctsCredits = 5 });
        }

        // 2. Dodajemy testowego wykładowcę
        var lecturerId = Guid.Parse("a1b2c3d4-e5f6-1111-2222-333333333333");
        if (!await _context.Lecturers.AnyAsync(l => l.Id == lecturerId))
        {
            _context.Lecturers.Add(new Lecturer { Id = lecturerId, FirstName = "Jan", LastName = "Kowalski", NationalId = new Pesel("01234567897"), Email = "jan.lec@wsei.edu.pl", Title = "dr inż.", Faculty = "IT" });
        }

        // 3. Dodajemy testowego studenta
        var studentId = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f");
        if (!await _context.Students.AnyAsync(s => s.Id == studentId))
        {
            _context.Students.Add(new Student { Id = studentId, FirstName = "Adam", LastName = "Nowak", NationalId = new Pesel("99010112342"), Email = "adam@wsei.edu.pl", YearOfStudy = 2, ProgramName = "Informatyka", Status = StudentStatus.Active });
        }

        await _context.SaveChangesAsync();
    }
}
