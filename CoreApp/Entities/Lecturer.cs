namespace CoreApp.Entities;

public class Lecturer : Person
{
    public string Title { get; set; } = string.Empty;
    public string Faculty { get; set; } = string.Empty;
    
    public ICollection<Course> TaughtCourses { get; set; } = new List<Course>();
}