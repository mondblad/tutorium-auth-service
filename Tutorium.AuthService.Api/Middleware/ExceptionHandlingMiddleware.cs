using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Tutorium.Shared.Utils.Exceptions;

namespace Tutorium.AuthService.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionMapper _mapper;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, IExceptionMapper mapper, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (AppException ex)
            {
                _logger.LogError(ex, "Handled exception: {ErrorCode}", ex.ErrorCode);

                context.Response.StatusCode = _mapper.Map(ex);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new { 
                    error = new {
                        code = ex.ErrorCode,
                        message = ex.Message,
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception");

                context.Response.StatusCode = 500;
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsJsonAsync(new
                {
                    error = new
                    {
                        code = "internal_error",
                        message = "Internal server error",
                        status = 500
                    }
                });
            }
        }
    }
}
