﻿using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class InMemoryLecturerRepository 
    : MemoryGenericRepository<Lecturer>, ILecturerRepository
{
    public InMemoryLecturerRepository() : base()
    {
        var lecturer1Id = Guid.Parse("a1b2c3d4-e5f6-1111-2222-333333333333");
        _data.Add(lecturer1Id, new Lecturer
        {
            Id = lecturer1Id,
            FirstName = "Jan",
            LastName = "Kowalski",
            Email = "jan.kowalski.lec@example.com",
            Title = "dr inż.",
            Faculty = "Wydział Informatyki"
        });
    }

    public Task<IEnumerable<Lecturer>> FindByCourse(Guid courseId)
    {
        var result = _data.Values.Where(l =>
            l.TaughtCourses != null &&
            l.TaughtCourses.Any(c => c.Id == courseId))
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Lecturer>> FindByTitle(string title)
    {
        var result = _data.Values.Where(l =>
            !string.IsNullOrWhiteSpace(l.Title) &&
            l.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }

    public Task<IEnumerable<Lecturer>> FindByFacultyAsync(string faculty)
    {
        var result = _data.Values.Where(l =>
            !string.IsNullOrWhiteSpace(l.Faculty) &&
            l.Faculty.Equals(faculty, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }
}
