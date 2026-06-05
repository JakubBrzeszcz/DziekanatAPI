﻿namespace CoreApp.Entities;

public abstract class Person : EntityBase
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public CoreApp.ValueObjects.Pesel NationalId { get; set; } = null!;
    public string Email { get; set; } = string.Empty;
}