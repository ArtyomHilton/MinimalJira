using FluentValidation;
using MinimalJira.Host.DTOs.Request;

namespace MinimalJira.Host.Validators;

public class UpdateProjectRequestValidator : AbstractValidator<UpdateProjectRequest>
{
    public UpdateProjectRequestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("Название проекта не должно быть пустым!")
            .MaximumLength(150)
            .WithMessage("Название проекта не должно быть более 150 символов!");

        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Описание проекта не должно быть пустым!");
    }
}