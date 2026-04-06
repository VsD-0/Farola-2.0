using Farola.Application.Common.Models;
using Farola.Application.Features.Auth.Commands.Login;
using Farola.Application.Features.Auth.Commands.RefreshToken;
using Farola.WebApi.Examples.Auth.Login;
using Farola.WebApi.Examples.Auth.Refresh;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Controllers
{
    /// <summary>
    /// Контроллер для аутентификации и управления токенами.
    /// </summary>
    /// <remarks>
    /// Предоставляет эндпоинты для входа пользователя (получение access/refresh токенов)
    /// и обновления access токена с использованием refresh токена.
    /// </remarks>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Аутентификация пользователя.
        /// </summary>
        /// <param name="command">Данные для входа (email, пароль, deviceId, deviceName).</param>
        /// <returns>Access token в теле ответа. Refresh token устанавливается в HttpOnly cookie.</returns>
        /// <response code="200">Успешный вход. Возвращает access token.</response>
        /// <response code="400">Ошибка валидации (неверный формат email, короткий пароль и т.п.).</response>
        /// <response code="401">Неверные учётные данные.</response>
        /// <response code="429">Превышен лимит попыток входа (5 в минуту).</response>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AccessTokenResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(LoginResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(ValidationErrorResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(LoginUnauthorizedErrorResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status429TooManyRequests, typeof(TooManyRequestsErrorResponseExample))]
        [SwaggerRequestExample(typeof(LoginCommand), typeof(LoginRequestExample))]
        public async Task<ActionResult<AccessTokenResult>> Login(LoginCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }

        /// <summary>
        /// Обновление access токена с использованием refresh токена.
        /// </summary>
        /// <remarks>
        /// Refresh token должен быть передан в HttpOnly cookie с именем "refreshToken". 
        /// Тело запроса не требуется.
        /// </remarks>
        /// <response code="200">Успешное обновление. Возвращает новый access token.</response>
        /// <response code="400">Ошибка валидации (например, отсутствует cookie).</response>
        /// <response code="401">Refresh token недействителен, истёк или отсутствует.</response>
        /// <response code="429">Превышен лимит запросов (10 в минуту).</response>
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(AccessTokenResult), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RefreshTokenResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(RefreshTokenBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status401Unauthorized, typeof(RefreshTokenUnauthorizedExample))]
        [SwaggerResponseExample(StatusCodes.Status429TooManyRequests, typeof(RefreshTokenTooManyRequestsExample))]
        public async Task<ActionResult<AccessTokenResult>> Refresh(RefreshTokenCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
    }
}
