using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace RoadmapPlusPlus.BuildingBlocks.Persistence
{
    /// <summary>
    /// <see cref="NpgsqlDataSource"/> берётся внутри проверки, а не в конструкторе: тогда и пустая строка подключения
    /// даёт 503 Unhealthy, а не 500 при создании проверки.
    /// </summary>
    internal sealed class PostgresHealthCheck( IServiceProvider services ) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync( HealthCheckContext context, CancellationToken cancellationToken = default )
        {
            NpgsqlDataSource dataSource = services.GetRequiredService<NpgsqlDataSource>();

            await using NpgsqlCommand command = dataSource.CreateCommand( "SELECT 1" );
            await command.ExecuteScalarAsync( cancellationToken );

            return HealthCheckResult.Healthy();
        }
    }
}
