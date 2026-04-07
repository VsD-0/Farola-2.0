using Farola.Application.DTOs.Sessions;
using Farola.Application.Features.Sessions.Commands.RevokeAllOtherSessions;
using Farola.Application.Features.Sessions.Commands.RevokeSession;
using Farola.Application.Features.Sessions.Queries.GetSessions;
using Farola.WebApi.Attributes;
using Farola.WebApi.Examples.Sessions.GetSessions;
using Farola.WebApi.Examples.Sessions.Revoke;
using Farola.WebApi.Examples.Sessions.RevokeAllOtherSessions;
using Farola.WebApi.Filters;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Controllers
{
    /// <summary>
    /// Управление активными сессиями (устройствами) пользователя.
    /// </summary>
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

        /// <summary>
        /// Получить список всех активных сессий (устройств) текущего пользователя.
        /// </summary>
        /// <remarks>
        /// Обязательный заголовок: X-Device-Id (UUID текущего устройства).
        /// </remarks>
        /// <response code="200">Список сессий.</response>
        /// <response code="400">Отсутствует заголовок X-Device-Id или он невалиден.</response>
        /// <response code="401">Пользователь не авторизован.</response>
        [HttpGet]
        [ProducesResponseType(typeof(List<SessionDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(MissingDeviceIdErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(GetSessionsUnauthorizedErrorExample))]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetSessionsResponseExample))]
        public async Task<ActionResult<List<SessionDto>>> GetSessions()
        {
            var currentDeviceId = HttpContext.Items["DeviceId"]?.ToString();
            var query = new GetSessionsQuery(currentDeviceId!);
            var sessions = await _mediator.Send(query);
            return Ok(sessions);
        }

        /// <summary>
        /// Отозвать конкретную сессию (устройство) по его DeviceId.
        /// </summary>
        /// <remarks>
        /// Требуется подтверждение паролем пользователя.
        /// </remarks>
        /// <param name="command">DeviceId сессии и пароль.</param>
        /// <response code="204">Сессия успешно отозвана.</response>
        /// <response code="400">Неверный запрос (отсутствует DeviceId или пароль).</response>
        /// <response code="401">Неверный пароль или пользователь не авторизован.</response>
        /// <response code="404">Сессия с указанным DeviceId не найдена.</response>
        [HttpPost("revoke")]
        [ValidateAjax]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status204NoContent, typeof(RevokeSessionSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RevokeSessionBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(RevokeSessionUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(RevokeSessionNotFoundExample))]
        [SwaggerRequestExample(typeof(RevokeSessionCommand), typeof(RevokeSessionRequestExample))]
        public async Task<IActionResult> RevokeSession(RevokeSessionCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }

        /// <summary>
        /// Отозвать все другие сессии, кроме текущей.
        /// </summary>
        /// <param name="command">Пароль пользователя для подтверждения.</param>
        /// <response code="204">Все другие сессии успешно отозваны.</response>
        /// <response code="400">Неверный запрос (отсутствует пароль).</response>
        /// <response code="401">Неверный пароль или пользователь не авторизован.</response>
        [HttpPost("revoke-all")]
        [ValidateAjax]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [SwaggerRequestExample(typeof(RevokeAllOtherSessionsCommand), typeof(RevokeAllOtherSessionsRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status204NoContent, typeof(RevokeAllOtherSessionsSuccessExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RevokeAllOtherSessionsBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(RevokeAllOtherSessionsUnauthorizedExample))]
        public async Task<IActionResult> RevokeAllOtherSessions(RevokeAllOtherSessionsCommand command)
        {
            await _mediator.Send(command);
            return NoContent();
        }
    }
}
