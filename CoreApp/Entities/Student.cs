﻿using CoreApp.Enums;

namespace CoreApp.Entities;

public class Student : Person
{
    public int YearOfStudy { get; set; }
    public string ProgramName { get; set; } = string.Empty;
    public int EnrollmentYearValue { get; set; }
    
    public DegreeProgram? DegreeProgram { get; set; }
    public AcademicYear? EnrollmentYear { get; set; }
    public StudentStatus Status { get; set; }
    
    public ICollection<Grade> Grades { get; set; } = new List<Grade>();
}