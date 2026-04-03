using Farola.Application.DTOs.Sessions.Sessions;
using Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions;
using Farola.Application.Features.Sessions.Commands.RevokeSession;
using Farola.Application.Features.Sessions.Queries.GetSessions;
using Farola.WebApi.Attributes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Farola.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    [RequireDeviceId]
    public class SessionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SessionsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<List<SessionDto>>> GetSessions()
        {
            var currentDeviceId = HttpContext.Items["DeviceId"]?.ToString();
            var query = new GetSessionsQuery(currentDeviceId!);
            var sessions = await _mediator.Send(query);
            return Ok(sessions);
        }

        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeSession(RevokeSessionCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        [HttpPost("revoke-all")]
        public async Task<IActionResult> RevokeAllOtherSessions(RevokeAllOtherSessionsCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
