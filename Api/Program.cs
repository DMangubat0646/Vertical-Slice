using Api.Features.Students;
using Api.Features.Courses;
using Api.Features.Admissions;
using Api.Features.Employees;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.Students.CreateStudent;
using Application.Features.Students.GetAllStudents;
using Application.Features.Students.GetStudentById;
using Application.Features.Courses.CreateCourse;
using Application.Features.Courses.GetAllCourses;
using Application.Features.Courses.GetCourseById;
using Application.Features.Admissions.CreateAdmission;
using Application.Features.Admissions.GetAdmissionById;

using Application.Features.Admissions.GetAllAdmissions;
using Application.Features.Employees.CreateEmployee;
using Application.Features.Employees.GetAllEmployees;
using Application.Features.Employees.GetEmployeeById;
using Application.Features.Employees.UpdateEmployee;
using Application.Features.Employees.DeleteEmployee;
using Application.Features.Courses.UpdateCourse;
using Application.Features.Courses.DeleteCourse;

using Domain.Common;
using FluentValidation;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<IAppDbContext, AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Database")));
// Validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateStudentValidator>();

// Handlers
builder.Services.AddScoped<ICommandHandler<CreateStudentCommand, Result<CreateStudentResponse>>, CreateStudentCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllStudentsQuery, Result<GetAllStudentsResponse>>, GetAllStudentsQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetStudentByIdQuery, Result<GetStudentByIdResponse>>, GetStudentByIdQueryHandler>();
builder.Services.AddScoped<ICommandHandler<CreateCourseCommand, Result<CreateCourseResponse>>, CreateCourseCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllCoursesQuery, Result<GetAllCoursesResponse>>, GetAllCoursesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetCourseByIdQuery, Result<GetCourseByIdResponse>>, GetCourseByIdQueryHandler>();
builder.Services.AddScoped<ICommandHandler<CreateAdmissionCommand, Result<CreateAdmissionResponse>>, CreateAdmissionCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetAdmissionByIdQuery, Result<GetAdmissionByIdResponse>>, GetAdmissionByIdQueryHandler>();

builder.Services.AddScoped<IQueryHandler<GetAllAdmissionsQuery, Result<GetAllAdmissionsResponse>>,GetAllAdmissionsQueryHandler>();

builder.Services.AddScoped<ICommandHandler<DeleteCourseCommand, Result>,DeleteCourseCommandHandler>();

builder.Services.AddScoped<ICommandHandler<UpdateCourseCommand, Result<UpdateCourseResponse>>,UpdateCourseCommandHandler>();
builder.Services.AddScoped<ICommandHandler<CreateEmployeeCommand, Result<CreateEmployeeResponse>>, CreateEmployeeCommandHandler>();
builder.Services.AddScoped<IQueryHandler<GetAllEmployeesQuery, Result<GetAllEmployeesResponse>>, GetAllEmployeesQueryHandler>();
builder.Services.AddScoped<IQueryHandler<GetEmployeeByIdQuery, Result<GetEmployeeByIdResponse>>, GetEmployeeByIdQueryHandler>();
builder.Services.AddScoped<ICommandHandler<UpdateEmployeeCommand, Result<UpdateEmployeeResponse>>, UpdateEmployeeCommandHandler>();
builder.Services.AddScoped<ICommandHandler<DeleteEmployeeCommand, Result>, DeleteEmployeeCommandHandler>();

// API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, _, _) =>
    {
        document.Info.Title = "Vertical Slice API";
        document.Info.Description = "A simple API demonstrating Vertical Slice Architecture";
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Students.AddRange(
        new Domain.Entities.Student
        {
            Id = Guid.NewGuid(),
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            DateOfBirth = new DateTime(2000, 1, 15)
        },
        new Domain.Entities.Student
        {
            Id = Guid.NewGuid(),
            FirstName = "Jane",
            LastName = "Smith",
            Email = "jane.smith@example.com",
            DateOfBirth = new DateTime(2001, 5, 20)
        }
    );

    dbContext.Employees.AddRange(
        new Domain.Entities.Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "Juan",
            LastName = "Dela Cruz",
            EmployeeNumber = "EMP-1001",
            Department = "Information Technology",
            HireDate = new DateOnly(2019, 6, 3),
            CreatedAt = DateTime.UtcNow
        },
        new Domain.Entities.Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "Liza",
            LastName = "Aquino",
            EmployeeNumber = "EMP-1002",
            Department = null,
            HireDate = new DateOnly(2021, 1, 11),
            CreatedAt = DateTime.UtcNow
        }
    );

    dbContext.Courses.AddRange(
        new Domain.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Introduction to Programming",
            Code = "CS101",
            Credits = 3
        },
        new Domain.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Data Structures",
            Code = "CS201",
            Credits = 4
        },
        new Domain.Entities.Course
        {
            Id = Guid.NewGuid(),
            Title = "Database Systems",
            Code = "CS301",
            Credits = 3
        }
    );

    dbContext.SaveChanges();
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

// Root endpoint
app.MapGet("/", () => "Welcome to the Vertical Slice API! Go to /scalar/v1 for API documentation.");

// Map endpoints
app.MapStudentEndpoints();
app.MapCourseEndpoints();
app.MapAdmissionEndpoints();
app.MapEmployeeEndpoints();

app.Run();
