namespace Farola.Application.Common.Models
{
    /// <summary>
    /// Результат успешной аутентификации – access токен.
    /// </summary>
    /// <param name="AccessToken">JWT access токен.</param>
    public record AccessTokenResult(string AccessToken);
}
