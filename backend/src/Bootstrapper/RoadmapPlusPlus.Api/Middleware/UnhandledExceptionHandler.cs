using Microsoft.AspNetCore.Diagnostics;

namespace RoadmapPlusPlus.Api.Middleware
{
    /// <summary>
    /// Последний рубеж для исключений. Битый запрос (<see cref="BadHttpRequestException"/>) отдаёт его статусом.
    /// Остальное пишет в лог с id запроса и отдаёт 500 без внутренностей; текст исключения виден только в Development.
    /// </summary>
    internal sealed class UnhandledExceptionHandler(
        IProblemDetailsService problemDetails,
        IHostEnvironment environment,
        ILogger<UnhandledExceptionHandler> logger ) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync( HttpContext httpContext, Exception exception, CancellationToken cancellationToken )
        {
            int status;
            string? detail;

            if ( exception is BadHttpRequestException badRequest )
            {
                status = badRequest.StatusCode;
                detail = badRequest.Message;
            }
            else
            {
                logger.LogError(
                    exception,
                    "Необработанное исключение в запросе {RequestId}: {Method} {Path}",
                    httpContext.TraceIdentifier,
                    httpContext.Request.Method,
                    httpContext.Request.Path );

                status = StatusCodes.Status500InternalServerError;
                detail = environment.IsDevelopment() ? exception.Message : null;
            }

            httpContext.Response.StatusCode = status;

            return await problemDetails.TryWriteAsync( new ProblemDetailsContext
            {
                HttpContext = httpContext,
                Exception = exception,
                ProblemDetails = { Status = status, Detail = detail },
            } );
        }
    }
}
