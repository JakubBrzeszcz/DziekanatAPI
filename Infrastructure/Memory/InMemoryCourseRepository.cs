using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class InMemoryCourseRepository : MemoryGenericRepository<Course>, ICourseRepository
{
    public InMemoryCourseRepository() : base()
    {
        var course1Id = Guid.Parse("c1a2b3d4-e5f6-7890-1234-567890abcdef");
        _data.Add(course1Id, new Course
        {
            Id = course1Id,
            Name = "Programowanie Obiektowe",
            EctsCredits = 5
        });
        var course2Id = Guid.Parse("d2b3c4d5-f6e7-8901-2345-67890abcdef1");
        _data.Add(course2Id, new Course
        {
            Id = course2Id,
            Name = "Bazy Danych",
            EctsCredits = 4
        });
    }
    public Task<IEnumerable<Course>> FindByDegreeProgramAsync(string programCode)
    {
        var result = _data.Values
            .Where(c => c.DegreeProgram != null && 
                        c.DegreeProgram.Code.Equals(programCode, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }
}