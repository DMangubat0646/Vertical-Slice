namespace Application.Features.Employees.CreateEmployee;

using Application.Abstractions.Messaging;
using Domain.Common;

public sealed record CreateEmployeeCommand(
    string FirstName,
    string LastName,
    string EmployeeNumber,
    string? Department,
    DateOnly HireDate
) : ICommand<Result<CreateEmployeeResponse>>;

public sealed record CreateEmployeeResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string EmployeeNumber,
    string? Department,
    DateOnly HireDate,
    DateTime CreatedAt
);
