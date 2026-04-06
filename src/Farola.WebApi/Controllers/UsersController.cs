using Farola.Application.DTOs.Users;
using Farola.Application.Features.Users.Commands.CreateUser;
using Farola.Application.Features.Users.Queries.GetUserById;
using Farola.Domain.Entities;
using Farola.WebApi.Examples.Sessions.GetSessions;
using Farola.WebApi.Examples.Users.CreateUser;
using Farola.WebApi.Examples.Users.GetUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Farola.WebApi.Controllers
{
    /// <summary>
    /// Управление пользователями.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Создать нового пользователя.
        /// </summary>
        /// <param name="command">Данные для регистрации.</param>
        /// <response code="201">Пользователь успешно создан. Возвращает ID пользователя и заголовок Location.</response>
        /// <response code="400">Ошибка валидации (неверный email, короткий пароль и т.п.).</response>
        /// <response code="409">Пользователь с таким email или телефоном уже существует.</response>
        [HttpPost]
        [ProducesResponseType(typeof(int), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [SwaggerRequestExample(typeof(CreateUserCommand), typeof(CreateUserRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status201Created, typeof(CreateUserResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status400BadRequest, typeof(CreateUserBadRequestExample))]
        [SwaggerResponseExample(StatusCodes.Status409Conflict, typeof(CreateUserConflictExample))]
        public async Task<ActionResult<int>> CreateUser(CreateUserCommand command)
        {
            var userId = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetUser), new { id = userId }, userId);
        }

        /// <summary>
        /// Получить пользователя по ID.
        /// </summary>
        /// <param name="id">Идентификатор пользователя.</param>
        /// <response code="200">Пользователь найден.</response>
        /// <response code="401">Не авторизован.</response>
        /// <response code="404">Пользователь с указанным ID не найден.</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(UserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [SwaggerResponseExample(StatusCodes.Status200OK, typeof(GetUserResponseExample))]
        [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(UserNotFoundExample))]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var query = new GetUserByIdQuery(id);
            var user = await _mediator.Send(query);
            if (user == null) return NotFound();
            return Ok(user);
        }
    }
}
