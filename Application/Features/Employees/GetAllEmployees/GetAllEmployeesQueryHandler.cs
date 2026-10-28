namespace Application.Features.Employees.GetAllEmployees;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

public sealed class GetAllEmployeesQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetAllEmployeesQuery, Result<GetAllEmployeesResponse>>
{
    public async Task<Result<GetAllEmployeesResponse>> HandleAsync(
        GetAllEmployeesQuery query,
        CancellationToken cancellationToken = default)
    {
        var employees = await dbContext.Employees
            .Select(e => new EmployeeResponse(
                e.Id,
                e.FirstName,
                e.LastName,
                e.EmployeeNumber,
                e.Department,
                e.HireDate,
                e.CreatedAt))
            .ToListAsync(cancellationToken);

        return Result<GetAllEmployeesResponse>.Success(
            new GetAllEmployeesResponse(employees));
    }
}
