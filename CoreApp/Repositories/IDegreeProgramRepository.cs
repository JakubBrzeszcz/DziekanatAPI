using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface IDegreeProgramRepository : IGenericRepositoryAsync<DegreeProgram>
{
    Task<DegreeProgram?> FindByCodeAsync(string code);
    Task<IEnumerable<DegreeProgram>> FindByFacultyAsync(string faculty);
}
