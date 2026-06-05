﻿﻿﻿using CoreApp.Enums;

namespace CoreApp.Entities;

public class Grade : EntityBase
{
    public Guid StudentId { get; set; }
    public Student Student { get; set; } = null!;
    public Guid CourseId { get; set; }
    public Course Course { get; set; } = null!;
    public Guid LecturerId { get; set; }
    public Lecturer Lecturer { get; set; } = null!;
    public GradeValue GradeValue { get; set; }
    public GradeType Type { get; set; }
    public DateTime DateOfIssue { get; set; }
}