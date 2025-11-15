namespace MinimalJira.Host.DTOs.Response;

/// <summary>
/// Ответ с ошибкой.
/// </summary>
/// <param name="StatusCode">Статус код.</param>
/// <param name="Message">Сообщение</param>
/// <param name="Property">Свойство, которое выдало ошибку.</param>
public record ErrorResponse(int StatusCode, string Message, string Property);