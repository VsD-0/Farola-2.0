using Farola.Application.Common.Models;
using MediatR;

namespace Farola.Application.Features.Auth.Commands.Login
{
    public record LoginCommand(string Email, string Password, string DeviceId, string DeviceName) : IRequest<AccessTokenResult>;
}
