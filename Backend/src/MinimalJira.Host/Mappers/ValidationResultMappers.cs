using FluentValidation.Results;
using MinimalJira.Host.DTOs.Response;

namespace MinimalJira.Host.Mappers;

public static class ValidationResultMappers
{
    public static ErrorResponse ToResponse(this ValidationResult result)
        => new ErrorResponse(StatusCodes.Status400BadRequest,
            result.Errors[0].ErrorMessage,
            result.Errors[0].PropertyName);
}