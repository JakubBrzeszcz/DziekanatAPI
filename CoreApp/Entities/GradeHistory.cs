namespace CoreApp.Entities;

public class GradeHistory
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public Guid GradeId { get; init; }
    public Grade Grade { get; init; } = null!;
    
    public double OldValue { get; init; }
    public double NewValue { get; init; }
    
    public string ChangedByUserId { get; init; } = string.Empty;
    public DateTime ChangedAt { get; init; } = DateTime.UtcNow;
    public string Action { get; init; } = string.Empty; // np. "Created", "Updated"
}