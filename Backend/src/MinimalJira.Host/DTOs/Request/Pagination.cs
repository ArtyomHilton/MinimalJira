namespace MinimalJira.Host.DTOs.Request;

public record Pagination(int Page = 1, int PageSize = 10);