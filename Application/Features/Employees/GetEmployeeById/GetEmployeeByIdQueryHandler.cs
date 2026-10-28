namespace Application.Features.Employees.GetEmployeeById;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

public sealed class GetEmployeeByIdQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetEmployeeByIdQuery, Result<GetEmployeeByIdResponse>>
{
    public async Task<Result<GetEmployeeByIdResponse>> HandleAsync(
        GetEmployeeByIdQuery query,
        CancellationToken cancellationToken = default)
    {
        var employee = await dbContext.Employees
            .FirstOrDefaultAsync(e => e.Id == query.Id, cancellationToken);

        if (employee is null)
        {
            return Result<GetEmployeeByIdResponse>.Failure(
                Error.NotFound("Employee.NotFound", "Employee not found."));
        }

        return Result<GetEmployeeByIdResponse>.Success(
            new GetEmployeeByIdResponse(
                employee.Id,
                employee.FirstName,
                employee.LastName,
                employee.EmployeeNumber,
                employee.Department,
                employee.HireDate,
                employee.CreatedAt));
    }
}
