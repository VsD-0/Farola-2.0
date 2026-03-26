using MediatR;
using Farola.Domain.Interfaces.Repositories;
using Farola.Domain.Interfaces.Services;

namespace Farola.Application.Features.Auth.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResult>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;
        private readonly IRefreshTokenRepository _refreshTokenRepository;

        public LoginCommandHandler(
            IUserRepository userRepository,
            ITokenService tokenService,
            IRefreshTokenRepository refreshTokenRepository)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
            _refreshTokenRepository = refreshTokenRepository;
        }

        public async Task<LoginResult> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByEmailAsync(request.Email);
            if (user == null)
                throw new UnauthorizedAccessException("Invalid credentials");

            // TODO: заменить на хэширование пароля
            if (user.Password != request.Password)
                throw new UnauthorizedAccessException("Invalid credentials");

            // Генерируем токены
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            // Сохраняем refresh token в БД
            var refreshTokenEntity = new Domain.Entities.RefreshToken
            {
                UserId = user.Id,
                Token = refreshToken,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7) // срок жизни
            };
            await _refreshTokenRepository.AddAsync(refreshTokenEntity);

            return new LoginResult(accessToken, refreshToken);
        }
    }
}
