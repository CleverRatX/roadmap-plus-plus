using Microsoft.Extensions.Diagnostics.HealthChecks;
using Npgsql;

namespace RoadmapPlusPlus.BuildingBlocks.Persistence
{
    internal sealed class PostgresHealthCheck( NpgsqlDataSource dataSource ) : IHealthCheck
    {
        public async Task<HealthCheckResult> CheckHealthAsync( HealthCheckContext context, CancellationToken cancellationToken = default )
        {
            await using NpgsqlCommand command = dataSource.CreateCommand( "SELECT 1" );
            await command.ExecuteScalarAsync( cancellationToken );

            return HealthCheckResult.Healthy();
        }
    }
}
