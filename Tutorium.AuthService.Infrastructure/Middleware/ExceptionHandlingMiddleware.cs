using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tutorium.AuthService.Core.Exceptions;

namespace Tutorium.AuthService.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            int statusCode;
            string message = ex.Message;

            switch (ex)
            {
                case UserAlreadyExistsException:
                    statusCode = StatusCodes.Status409Conflict;
                    break;
                case InvalidEmailException:
                case InvalidPasswordException:
                case InvalidConfirmationCodeException:
                    statusCode = StatusCodes.Status400BadRequest;
                    break;
                case RegistrationAttemptNotFoundException:
                    statusCode = StatusCodes.Status404NotFound;
                    break;
                default:
                    statusCode = StatusCodes.Status500InternalServerError;
                    message = "Internal server error";
                    _logger.LogError(ex, "Unhandled exception");
                    break;
            }

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = System.Text.Json.JsonSerializer.Serialize(new { error = message });
            return context.Response.WriteAsync(result);
        }
    }
}
