﻿using CoreApp.Entities;
using CoreApp.Enums;
using CoreApp.Repositories;
using CoreApp.ValueObjects;

namespace Infrastructure.Memory;

public class InMemoryStudentRepository : MemoryGenericRepository<Student>, IStudentRepository
{
    public InMemoryStudentRepository() : base()
    {
        var student1Id = Guid.Parse("3d54091d-abc8-49ec-9590-93ad3ed5458f");
        _data.Add(student1Id, new Student
        {
            Id = student1Id,
            FirstName = "Adam",
            LastName = "Nowak",
            NationalId = new Pesel("99010112342"), // Zmieniono ostatnią cyfrę, aby suma kontrolna była poprawna
            Email = "adam.nowak@example.com",
            YearOfStudy = 2,
            ProgramName = "Informatyka",
            Status = StudentStatus.Active
        });

        var student2Id = Guid.Parse("8a5c6f8e-7d4a-4b1c-9c9b-9e3e2a1b0f7c");
        _data.Add(student2Id, new Student
        {
            Id = student2Id,
            FirstName = "Ewa",
            LastName = "Kowalska",
            NationalId = new Pesel("01234567897"), // Zmieniono ostatnią cyfrę, aby suma kontrolna była poprawna
            Email = "ewa.kowalska@example.com",
            YearOfStudy = 3,
            ProgramName = "Informatyka",
            Status = StudentStatus.Active
        });
    }

    public Task<IEnumerable<Student>> FindByYearOfStudyAsync(int year)
    {
        var result = _data.Values
            .Where(s => s.YearOfStudy == year)
            .ToList()
            .AsEnumerable();
            
        return Task.FromResult(result);
    }

    public Task<IEnumerable<Student>> FindByProgramAsync(string programCode)
    {
        var result = _data.Values
            .Where(s => s.DegreeProgram != null && 
                        s.DegreeProgram.Code.Equals(programCode, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsEnumerable();
            
        return Task.FromResult(result);
    }

    public Task ChangeStatusAsync(Guid studentId, StudentStatus status)
    {
        if (!_data.TryGetValue(studentId, out var student))
            throw new KeyNotFoundException($"Student with id {studentId} was not found.");

        student.Status = status;
        return Task.CompletedTask;
    }
}