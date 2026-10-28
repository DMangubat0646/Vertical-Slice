namespace Application.Features.Employees.DeleteEmployee;

using FluentValidation;

public sealed class DeleteEmployeeValidator : AbstractValidator<DeleteEmployeeCommand>
{
    public DeleteEmployeeValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}
