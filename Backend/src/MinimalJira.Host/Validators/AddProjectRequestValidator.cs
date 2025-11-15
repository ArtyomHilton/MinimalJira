using FluentValidation;
using MinimalJira.Host.DTOs.Request;

namespace MinimalJira.Host.Validators;

public class AddProjectRequestValidator : AbstractValidator<AddProjectRequest>
{
    public AddProjectRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;
        
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