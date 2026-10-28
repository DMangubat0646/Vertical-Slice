namespace Application.Features.Employees.DeleteEmployee;

using Application.Abstractions.Messaging;
using Domain.Common;

public sealed record DeleteEmployeeCommand(Guid Id) : ICommand<Result>;
