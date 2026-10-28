namespace Application.Features.Employees.UpdateEmployee;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public sealed class UpdateEmployeeCommandHandler(
    IAppDbContext dbContext,
    IValidator<UpdateEmployeeCommand> validator)
    : ICommandHandler<UpdateEmployeeCommand, Result<UpdateEmployeeResponse>>
{
    public async Task<Result<UpdateEmployeeResponse>> HandleAsync(
        UpdateEmployeeCommand command,
        CancellationToken cancellationToken = default)
    {
        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
        {
            return Result<UpdateEmployeeResponse>.Failure(
                Error.Validation(
                    "Employee.Validation",
                    string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));
        }

        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

        if (employee is null)
        {
            return Result<UpdateEmployeeResponse>.Failure(
                Error.NotFound("Employee.NotFound", "Employee not found."));
        }

        var numberTaken = await dbContext.Employees
            .AnyAsync(
                e => e.EmployeeNumber == command.EmployeeNumber && e.Id != command.Id,
                cancellationToken);

        if (numberTaken)
        {
            return Result<UpdateEmployeeResponse>.Failure(
                Error.Conflict(
                    "Employee.EmployeeNumberConflict",
                    $"An employee with number '{command.EmployeeNumber}' already exists."));
        }

        employee.FirstName = command.FirstName;
        employee.LastName = command.LastName;
        employee.EmployeeNumber = command.EmployeeNumber;
        employee.Department = command.Department;
        employee.HireDate = command.HireDate;

        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<UpdateEmployeeResponse>.Success(
            new UpdateEmployeeResponse(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.EmployeeNumber,
                employee.Department,
                employee.HireDate,
                employee.CreatedAt));
    }
}
