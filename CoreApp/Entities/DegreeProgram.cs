namespace CoreApp.Entities;

public class DegreeProgram : EntityBase
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public int MinEctsForDiploma { get; set; }
    public string Faculty { get; set; } = string.Empty;
}