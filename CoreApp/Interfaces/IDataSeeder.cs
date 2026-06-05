namespace CoreApp.Interfaces;

public interface IDataSeeder
{
    int Order { get; }
    Task SeedAsync();
}