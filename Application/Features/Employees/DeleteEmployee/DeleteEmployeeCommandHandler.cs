namespace Application.Features.Employees.DeleteEmployee;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

public sealed class DeleteEmployeeCommandHandler(
    IAppDbContext dbContext,
    IValidator<DeleteEmployeeCommand> validator)
    : ICommandHandler<DeleteEmployeeCommand, Result>
{
    public async Task<Result> HandleAsync(
        DeleteEmployeeCommand command,
        CancellationToken cancellationToken = default)
    {
        var validation = await validator.ValidateAsync(command, cancellationToken);
        if (!validation.IsValid)
        {
            return Result.Failure(
                Error.Validation(
                    "Employee.Validation",
                    string.Join("; ", validation.Errors.Select(e => e.ErrorMessage))));
        }

        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == command.Id, cancellationToken);

        if (employee is null)
        {
            return Result.Failure(
                Error.NotFound("Employee.NotFound", "Employee not found."));
        }

        dbContext.Employees.Remove(employee);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
