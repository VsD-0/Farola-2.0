namespace Farola.Application.DTOs.Sessions.Sessions
{
    public record SessionDto(
    int Id,
    string DeviceId,
    string DeviceName,
    DateTime CreatedAt,
    DateTime ExpiresAt,
    string IpAddress,
    string UserAgent,
    bool IsCurrentDevice);
}
