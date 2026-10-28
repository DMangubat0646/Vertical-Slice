namespace Application.Features.Employees.UpdateEmployee;

using FluentValidation;

public sealed class UpdateEmployeeValidator : AbstractValidator<UpdateEmployeeCommand>
{
    public UpdateEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.FirstName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.LastName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.EmployeeNumber).NotEmpty().MaximumLength(20);
        RuleFor(x => x.Department).MaximumLength(100);
        RuleFor(x => x.HireDate)
            .NotEmpty()
            .LessThanOrEqualTo(DateOnly.FromDateTime(DateTime.Today));
    }
}
