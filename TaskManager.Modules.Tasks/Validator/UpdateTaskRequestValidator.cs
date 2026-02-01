using FluentValidation;
using TaskManager.Modules.Tasks.DTOs;

namespace TaskManager.Modules.Tasks.Validator;

public class UpdateTaskRequestValidator : AbstractValidator<UpdateTaskRequest>
{
    public UpdateTaskRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .NotEmpty()
            .MinimumLength(10);

        RuleFor(x => x.Priority)
            .NotEmpty()
            .Must(p => new[] { "Low", "Medium", "High" }.Contains(p))
            .WithMessage("Priority must be Low, Medium, or High");

        RuleFor(x => x.DueDate)
            .Must(d => d == null || d.Value.Date >= DateTime.UtcNow.Date)
            .WithMessage("Due date cannot be in the past");
    }
}
