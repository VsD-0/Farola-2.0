using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;

namespace Farola.WebApi.Filters
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var exception = context.Exception;

            if (exception is ValidationException validationException)
            {
                context.Result = new BadRequestObjectResult(new
                {
                    title = "Validation Error",
                    status = 400,
                    errors = validationException.Errors
                        .GroupBy(e => e.PropertyName)
                        .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
                });
                context.ExceptionHandled = true;
                return;
            }

            if (exception is UnauthorizedAccessException)
            {
                context.Result = new UnauthorizedObjectResult(new
                {
                    title = "Unauthorized",
                    status = 401,
                    detail = exception.Message
                });
                context.ExceptionHandled = true;
                return;
            }

            if (exception is KeyNotFoundException ||
                exception is ArgumentException && exception.Message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new NotFoundObjectResult(new
                {
                    title = "Not Found",
                    status = 404,
                    detail = exception.Message
                });
                context.ExceptionHandled = true;
                return;
            }

            if (exception is DbUpdateException dbEx)
            {
                var message = dbEx.InnerException?.Message ?? dbEx.Message;
                context.Result = new BadRequestObjectResult(new
                {
                    title = "Database Error",
                    status = 400,
                    detail = message
                });
                context.ExceptionHandled = true;
                _logger.LogError(dbEx, "Database error occurred");
                return;
            }

            _logger.LogError(exception, "Unhandled exception occurred");
            context.Result = new ObjectResult(new
            {
                title = "Internal Server Error",
                status = 500,
                detail = "An unexpected error occurred. Please try again later."
            })
            {
                StatusCode = 500
            };
            context.ExceptionHandled = true;
        }
    }
}
