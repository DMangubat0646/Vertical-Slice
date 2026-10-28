namespace Application.Features.Employees.CreateEmployee;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Entities;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public sealed class CreateEmployeeCommandHandler(
    IAppDbContext dbContext,
    IValidator<CreateEmployeeCommand> validator)
    : ICommandHandler<CreateEmployeeCommand, Result<CreateEmployeeResponse>>
{
    public async Task<Result<CreateEmployeeResponse>> HandleAsync(
        CreateEmployeeCommand command,
        CancellationToken cancellationToken = default)
    {
        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
        {
            return Result<CreateEmployeeResponse>.Failure(
                Error.Validation(
                    "Employee.Validation",
                    string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));
        }

        var numberTaken = await dbContext.Employees
            .AnyAsync(e => e.EmployeeNumber == command.EmployeeNumber, cancellationToken);

        if (numberTaken)
        {
            return Result<CreateEmployeeResponse>.Failure(
                Error.Conflict(
                    "Employee.EmployeeNumberConflict",
                    $"An employee with number '{command.EmployeeNumber}' already exists."));
        }

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = command.FirstName,
            LastName = command.LastName,
            EmployeeNumber = command.EmployeeNumber,
            Department = command.Department,
            HireDate = command.HireDate,
            CreatedAt = DateTime.UtcNow
        };

        dbContext.Employees.Add(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<CreateEmployeeResponse>.Success(
            new CreateEmployeeResponse(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.EmployeeNumber,
                employee.Department,
                employee.HireDate,
                employee.CreatedAt));
    }
}
