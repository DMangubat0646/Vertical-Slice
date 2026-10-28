namespace Domain.Entities;

using Domain.Common;

public sealed class Employee : BaseEntity
{
    public string FirstName { get; set; } = default!;
    public string LastName { get; set; } = default!;
    public string EmployeeNumber { get; set; } = default!;
    public string? Department { get; set; }
    public DateOnly HireDate { get; set; }
    public DateTime CreatedAt { get; set; }
}
