using CoreApp.Entities;

namespace CoreApp.Repositories;

public interface ILecturerRepository : IGenericRepositoryAsync<Lecturer>
{
    Task<IEnumerable<Lecturer>> FindByCourse(Guid courseId);
    Task<IEnumerable<Lecturer>> FindByTitle(string title);
    Task<IEnumerable<Lecturer>> FindByFacultyAsync(string faculty);
}