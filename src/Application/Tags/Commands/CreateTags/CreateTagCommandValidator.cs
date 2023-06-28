using FluentValidation;

namespace Todo_App.Application.Tags.Commands.CreateTags;
public class CreateTagCommandValidator : AbstractValidator<CreateTagCommand>
{
    public CreateTagCommandValidator()
    {
        RuleFor(v => v.Name)
            .MinimumLength(1)
            .MaximumLength(200)
            .NotEmpty();
    }
}
