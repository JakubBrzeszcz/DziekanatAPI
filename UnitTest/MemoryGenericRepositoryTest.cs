using CoreApp.Entities;
using CoreApp.Repositories;
using Infrastructure.Memory;

namespace UnitTest;

public class MemoryGenericRepositoryTest
{
    private readonly IGenericRepositoryAsync<Student> _repo = new MemoryGenericRepository<Student>();

    [Fact]
    public async Task AddStudentToRepositoryTestAsync()
    {
        var expected = CreateStudent("Adam", "Nowak");

        await _repo.AddAsync(expected);

        var actual = await _repo.FindByIdAsync(expected.Id);
        Assert.Equal(expected, actual);
        Assert.Equal(expected.Id, actual?.Id);
    }

    [Fact]
    public async Task AddStudentShouldGenerateIdWhenIdIsEmpty()
    {
        var student = CreateStudent("Ewa", "Kowalska");
        student.Id = Guid.Empty;

        var result = await _repo.AddAsync(student);

        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(result, await _repo.FindByIdAsync(result.Id));
    }

    [Fact]
    public async Task AddStudentShouldThrowWhenIdAlreadyExists()
    {
        var student = CreateStudent("Adam", "Nowak");
        await _repo.AddAsync(student);

        await Assert.ThrowsAsync<InvalidOperationException>(() => _repo.AddAsync(student));
    }

    [Fact]
    public async Task FindByIdShouldReturnNullWhenStudentDoesNotExist()
    {
        var result = await _repo.FindByIdAsync(Guid.NewGuid());

        Assert.Null(result);
    }

    [Fact]
    public async Task FindAllShouldReturnAllStudents()
    {
        var s1 = CreateStudent("Adam", "Nowak");
        var s2 = CreateStudent("Jan", "Kowalski");

        await _repo.AddAsync(s1);
        await _repo.AddAsync(s2);

        var result = await _repo.FindAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(s1, result);
        Assert.Contains(s2, result);
    }

    [Fact]
    public async Task FindPagedShouldReturnCorrectPage()
    {
        for (var i = 0; i < 10; i++)
            await _repo.AddAsync(CreateStudent($"Student{i}", "Testowy"));

        var page = await _repo.FindPagedAsync(2, 3);

        Assert.Equal(3, page.Items.Count);
        Assert.Equal(10, page.TotalCount);
        Assert.Equal(2, page.Page);
        Assert.Equal(3, page.PageSize);
        Assert.Equal(4, page.TotalPages);
        Assert.True(page.HasNext);
        Assert.True(page.HasPrevious);
    }

    [Theory]
    [InlineData(0, 5)]
    [InlineData(1, 0)]
    public async Task FindPagedShouldThrowForInvalidArguments(int page, int pageSize)
    {
        await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => _repo.FindPagedAsync(page, pageSize));
    }

    [Fact]
    public async Task UpdateStudentShouldReplaceExistingStudent()
    {
        var student = CreateStudent("Adam", "Nowak");
        await _repo.AddAsync(student);

        student.LastName = "Kowalski";
        var updated = await _repo.UpdateAsync(student);

        var actual = await _repo.FindByIdAsync(student.Id);
        Assert.Equal(updated, actual);
        Assert.Equal("Kowalski", actual?.LastName);
    }

    [Fact]
    public async Task UpdateStudentShouldThrowWhenStudentDoesNotExist()
    {
        var student = CreateStudent("Adam", "Nowak");

        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.UpdateAsync(student));
    }

    [Fact]
    public async Task RemoveStudentShouldDeleteStudent()
    {
        var student = CreateStudent("Adam", "Nowak");
        await _repo.AddAsync(student);

        await _repo.RemoveByIdAsync(student.Id);

        var result = await _repo.FindByIdAsync(student.Id);
        Assert.Null(result);
    }

    [Fact]
    public async Task RemoveStudentShouldThrowWhenStudentDoesNotExist()
    {
        await Assert.ThrowsAsync<KeyNotFoundException>(() => _repo.RemoveByIdAsync(Guid.NewGuid()));
    }

    private static Student CreateStudent(string firstName, string lastName)
    {
        return new Student
        {
            Id = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            NationalId = "12345678901",
            Email = $"{firstName.ToLowerInvariant()}.{lastName.ToLowerInvariant()}@example.com",
            YearOfStudy = 1
        };
    }
}
