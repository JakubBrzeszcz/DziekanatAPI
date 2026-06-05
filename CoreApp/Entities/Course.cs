namespace CoreApp.Entities;

public class Course : EntityBase
{
    public string Name { get; set; } = string.Empty;
    public int EctsCredits { get; set; }
    
    public DegreeProgram? DegreeProgram { get; set; }
    public AcademicYear? AcademicYear { get; set; }
    public Lecturer? Lecturer { get; set; }
}