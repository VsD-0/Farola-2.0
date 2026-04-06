namespace Farola.Application.DTOs.Sessions.Sessions
{
    /// <summary>
    /// Информация об активной сессии (устройстве).
    /// </summary>
    public record SessionDto(
    /// <summary>Идентификатор записи токена.</summary>
    int Id,
    /// <summary>Уникальный идентификатор устройства.</summary>
    string DeviceId,
    /// <summary>Название устройства.</summary>
    string DeviceName,
    /// <summary>Дата и время создания сессии (UTC).</summary>
    DateTime CreatedAt,
    /// <summary>Дата и время истечения токена (UTC).</summary>
    DateTime ExpiresAt,
    /// <summary>IP-адрес, с которого установлена сессия.</summary>
    string IpAddress,
    /// <summary>User-Agent браузера/клиента.</summary>
    string UserAgent,
    /// <summary>Флаг, указывающий, является ли это текущее устройство.</summary>
    bool IsCurrentDevice);
}
