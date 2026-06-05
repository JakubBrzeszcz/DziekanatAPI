using CoreApp.Entities;
using CoreApp.Repositories;

namespace Infrastructure.Memory;

public class InMemoryDegreeProgramRepository
    : MemoryGenericRepository<DegreeProgram>, IDegreeProgramRepository
{
    public Task<DegreeProgram?> FindByCodeAsync(string code)
    {
        var result = _data.Values.FirstOrDefault(p =>
            p.Code.Equals(code, StringComparison.OrdinalIgnoreCase));

        return Task.FromResult(result);
    }

    public Task<IEnumerable<DegreeProgram>> FindByFacultyAsync(string faculty)
    {
        var result = _data.Values
            .Where(p => p.Faculty.Equals(faculty, StringComparison.OrdinalIgnoreCase))
            .ToList()
            .AsEnumerable();

        return Task.FromResult(result);
    }
}
