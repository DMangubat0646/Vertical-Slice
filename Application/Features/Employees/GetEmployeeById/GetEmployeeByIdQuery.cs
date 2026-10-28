namespace Application.Features.Employees.GetEmployeeById;

using Application.Abstractions.Messaging;
using Domain.Common;

public sealed record GetEmployeeByIdQuery(Guid Id)
    : IQuery<Result<GetEmployeeByIdResponse>>;

public sealed record GetEmployeeByIdResponse(
    Guid Id,
    string FirstName,
    string LastName,
    string EmployeeNumber,
    string? Department,
    DateOnly HireDate,
    DateTime CreatedAt
);
