namespace RoadmapPlusPlus.Api.Middleware
{
    /// <summary>
    /// Отдаёт id запроса в заголовке <c>X-Request-Id</c>. Это <see cref="HttpContext.TraceIdentifier"/> —
    /// тот же id, что ASP.NET пишет в логи как <c>RequestId</c> и что попадает в <c>requestId</c> у ProblemDetails.
    /// </summary>
    internal sealed class RequestIdMiddleware( RequestDelegate next )
    {
        public const string HeaderName = "X-Request-Id";

        public Task InvokeAsync( HttpContext context )
        {
            context.Response.OnStarting( static state =>
            {
                HttpContext httpContext = ( HttpContext )state;
                httpContext.Response.Headers[ HeaderName ] = httpContext.TraceIdentifier;
                return Task.CompletedTask;
            }, context );

            return next( context );
        }
    }
}
