using Farola.Application.DTOs.Sessions.Sessions;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Examples.Sessions.GetSessions
{
    public class GetSessionsResponseExample : IExamplesProvider<List<SessionDto>>
    {
        public List<SessionDto> GetExamples()
        {
            return new List<SessionDto>
        {
            new SessionDto(
                Id: 1,
                DeviceId: "f47ac10b-58cc-4372-a567-0e02b2c3d479",
                DeviceName: "Chrome на Windows 11",
                CreatedAt: DateTime.UtcNow.AddDays(-1),
                ExpiresAt: DateTime.UtcNow.AddDays(6),
                IpAddress: "192.168.1.1",
                UserAgent: "Mozilla/5.0 ...",
                IsCurrentDevice: true
            ),
            new SessionDto(
                Id: 2,
                DeviceId: "550e8400-e29b-41d4-a716-446655440000",
                DeviceName: "iPhone Safari",
                CreatedAt: DateTime.UtcNow.AddDays(-3),
                ExpiresAt: DateTime.UtcNow.AddDays(4),
                IpAddress: "10.0.0.1",
                UserAgent: "Mozilla/5.0 (iPhone; ...)",
                IsCurrentDevice: false
            )
        };
        }
    }
}
