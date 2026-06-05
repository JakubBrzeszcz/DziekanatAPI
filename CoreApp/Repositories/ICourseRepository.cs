using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface ICourseRepository : IGenericRepositoryAsync<Course>
{
    Task<IEnumerable<Course>> FindByDegreeProgramAsync(string programCode);
}