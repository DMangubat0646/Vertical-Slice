using Api.Common.Extensions;
using Application.Abstractions.Messaging;
using Application.Features.Employees.CreateEmployee;
using Application.Features.Employees.DeleteEmployee;
using Application.Features.Employees.GetAllEmployees;
using Application.Features.Employees.GetEmployeeById;
using Application.Features.Employees.UpdateEmployee;
using Domain.Common;

namespace Api.Features.Employees;

public static class EmployeeEndpoints
{
    public static void MapEmployeeEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/employees")
            .WithTags("Employees");

        group.MapPost("/", Create)
            .WithName("CreateEmployee")
            .WithSummary("Create an employee");

        group.MapGet("/", GetAll)
            .WithName("GetAllEmployees")
            .WithSummary("Get all employees");

        group.MapGet("/{id:guid}", GetById)
            .WithName("GetEmployeeById")
            .WithSummary("Get an employee by ID");

        group.MapPut("/{id:guid}", Update)
            .WithName("UpdateEmployee")
            .WithSummary("Update an employee");

        group.MapDelete("/{id:guid}", Delete)
            .WithName("DeleteEmployee")
            .WithSummary("Delete an employee");
    }

    private static async Task<IResult> Create(
        CreateEmployeeCommand command,
        ICommandHandler<CreateEmployeeCommand, Result<CreateEmployeeResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.CreatedAtRoute(
                result.Value,
                "GetEmployeeById",
                new { id = result.Value!.Id })
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetAll(
        IQueryHandler<GetAllEmployeesQuery, Result<GetAllEmployeesResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new GetAllEmployeesQuery(),
            cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> GetById(
        Guid id,
        IQueryHandler<GetEmployeeByIdQuery, Result<GetEmployeeByIdResponse>> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new GetEmployeeByIdQuery(id),
            cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> Update(
        Guid id,
        UpdateEmployeeRequest request,
        ICommandHandler<UpdateEmployeeCommand, Result<UpdateEmployeeResponse>> handler,
        CancellationToken cancellationToken)
    {
        var command = new UpdateEmployeeCommand(
            id,
            request.FirstName,
            request.LastName,
            request.EmployeeNumber,
            request.Department,
            request.HireDate);

        var result = await handler.HandleAsync(command, cancellationToken);

        return result.IsSuccess
            ? TypedResults.Ok(result.Value)
            : result.ToProblemDetails();
    }

    private static async Task<IResult> Delete(
        Guid id,
        ICommandHandler<DeleteEmployeeCommand, Result> handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            new DeleteEmployeeCommand(id),
            cancellationToken);

        return result.Match(
            onSuccess: () => TypedResults.NoContent(),
            onFailure: _ => result.ToProblemDetails());
    }

    public sealed record UpdateEmployeeRequest(
        string FirstName,
        string LastName,
        string EmployeeNumber,
        string? Department,
        DateOnly HireDate);
}
