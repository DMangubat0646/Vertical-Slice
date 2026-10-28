namespace Application.Features.Employees.UpdateEmployee;

using Application.Abstractions.Messaging;
using Domain.Common;

public sealed record UpdateEmployeeCommand(
    Guid Id,
    string FirstName,
    string LastName,
    string EmployeeNumber,
    string? Department,
    DateOnly HireDate
) : ICommand<Result<UpdateEmployeeResponse>>;

public sealed record UpdateEmployeeResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string EmployeeNumber,
    string? Department,
    DateOnly HireDate,
    DateTime CreatedAt
);
