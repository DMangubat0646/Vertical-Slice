namespace Application.Features.Employees.CreateEmployee;

using FluentValidation;

public sealed class CreateEmployeeValidator : AbstractValidator<CreateEmployeeCommand>
{
    public CreateEmployeeValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EmployeeNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Department).MaximumLength(100);
        RuleFor(x => x.HireDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
    }
}
