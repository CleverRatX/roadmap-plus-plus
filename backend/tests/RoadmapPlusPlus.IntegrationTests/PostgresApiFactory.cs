using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using RoadmapPlusPlus.BuildingBlocks.Persistence;
using RoadmapPlusPlus.IntegrationTests;
using Testcontainers.PostgreSql;

[assembly: AssemblyFixture( typeof( PostgresApiFactory ) )]

namespace RoadmapPlusPlus.IntegrationTests
{
    /// <summary>
    /// API целиком, в памяти процесса тестов, поверх настоящего Postgres в Docker-контейнере.
    /// Один на все тесты сборки: контейнер поднимается один раз, миграции модулей API применяет при старте, как в проде.
    /// </summary>
    public sealed class PostgresApiFactory : WebApplicationFactory<Program>, IAsyncLifetime
    {
        private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder( "postgres:18-alpine" ).Build();

        public async ValueTask InitializeAsync()
        {
            await _postgres.StartAsync();
        }

        public override async ValueTask DisposeAsync()
        {
            await base.DisposeAsync();
            await _postgres.DisposeAsync();
        }

        protected override void ConfigureWebHost( IWebHostBuilder builder )
        {
            builder.UseEnvironment( "Testing" );
            builder.UseSetting( $"ConnectionStrings:{PersistenceServiceCollectionExtensions.ConnectionStringName}", _postgres.GetConnectionString() );
        }
    }
}
