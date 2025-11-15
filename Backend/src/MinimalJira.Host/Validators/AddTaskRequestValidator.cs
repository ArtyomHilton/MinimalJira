using System.Data;
using FluentValidation;
using MinimalJira.Host.DTOs.Request;

namespace MinimalJira.Host.Validators;

public class AddTaskRequestValidator : AbstractValidator<AddTaskRequest>
{
    public AddTaskRequestValidator()
    {
        ClassLevelCascadeMode = CascadeMode.Stop;

        RuleFor(t => t.Title)
            .NotEmpty()
            .WithMessage("Название задачи не должно быть пустым!")
            .MaximumLength(100)
            .WithMessage("Название задачи не должно быть более 100 символов!");

        RuleFor(t => t.Description)
            .NotEmpty()
            .WithMessage("Описание задачи не должно быть пустым!");

        RuleFor(t => t.IsComplete)
            .NotNull()
            .WithMessage("Статус задачи обязателен!");

        RuleFor(t => t.ProjectId)
            .NotEmpty()
            .WithMessage("Идентификатор проекта обязателен!");
    }
}