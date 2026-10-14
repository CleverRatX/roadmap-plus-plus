namespace RoadmapPlusPlus.Api.Middleware
{
    internal static class ErrorHandlingExtensions
    {
        public const string RequestIdExtension = "requestId";

        /// <summary>
        /// Все ошибки отдаются как ProblemDetails с <c>requestId</c>. Стандартный <c>traceId</c> убираем,
        /// чтобы у запроса был один id и в ответе, и в заголовке, и в логах.
        /// </summary>
        public static IServiceCollection AddErrorHandling( this IServiceCollection services )
        {
            services.AddProblemDetails( options => options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Extensions.Remove( "traceId" );
                context.ProblemDetails.Extensions[ RequestIdExtension ] = context.HttpContext.TraceIdentifier;
            } );
            services.AddExceptionHandler<UnhandledExceptionHandler>();

            return services;
        }

        /// <summary>
        /// Порядок важен: id запроса — снаружи, чтобы попасть и в ответ с ошибкой;
        /// пустые ответы 404 / 405 тоже становятся ProblemDetails.
        /// </summary>
        public static IApplicationBuilder UseErrorHandling( this IApplicationBuilder app )
        {
            app.UseMiddleware<RequestIdMiddleware>();
            app.UseExceptionHandler();
            app.UseStatusCodePages();

            return app;
        }
    }
}
