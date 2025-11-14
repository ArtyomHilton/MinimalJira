namespace MinimalJira.Application.Interfaces;

/// <summary>
/// Сервис кэширования.
/// </summary>
public interface ICacheService
{
    /// <summary>
    /// Добавить значение.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="value">Значение.</param>
    /// <param name="expiresByMinutes">Время жизни в минутах.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <typeparam name="T">Любой тип.</typeparam>
    /// <returns><see cref="Task"/></returns>
    Task SetAsync<T>(string key, T value, int expiresByMinutes,CancellationToken cancellationToken);

    /// <summary>
    /// Получить значение.
    /// </summary>
    /// <param name="key">Ключ</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <typeparam name="T">Любой тип.</typeparam>
    /// <returns>Данные любого типа.</returns>
    Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken);

    /// <summary>
    /// Удалить значение.
    /// </summary>
    /// <param name="key">Ключ.</param>
    /// <param name="cancellationToken"><see cref="CancellationToken"/></param>
    /// <returns><see cref="Task"/></returns>
    Task DeleteAsync(string key, CancellationToken cancellationToken);
}