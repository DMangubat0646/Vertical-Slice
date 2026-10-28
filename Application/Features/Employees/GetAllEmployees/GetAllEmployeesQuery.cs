namespace Application.Features.Employees.GetAllEmployees;

using Application.Abstractions.Messaging;
using Domain.Common;

public sealed record GetAllEmployeesQuery()
    : IQuery<Result<GetAllEmployeesResponse>>;

public sealed record GetAllEmployeesResponse(
    List<EmployeeResponse> Employees
);

public sealed record EmployeeResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string EmployeeNumber,
    string? Department,
    DateOnly HireDate,
    DateTime CreatedAt
);
