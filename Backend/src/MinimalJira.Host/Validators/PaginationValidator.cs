using FluentValidation;
using MinimalJira.Host.DTOs.Request;

namespace MinimalJira.Host.Validators;

public class PaginationValidator : AbstractValidator<Pagination>
{
    public PaginationValidator()
    {
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(p => p.Page)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Номер страницы не может быть меньше 1");

        RuleFor(p => p.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Размер страницы не может быть меньше 1");
    }
}