using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using RoadmapPlusPlus.BuildingBlocks.Persistence;

namespace RoadmapPlusPlus.IntegrationTests
{
    /// <summary>
    /// API, у которого база недоступна: строка подключения ведёт на закрытый порт.
    /// </summary>
    public sealed class UnreachableDatabaseApiFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost( IWebHostBuilder builder )
        {
            builder.UseEnvironment( "Testing" );
            builder.UseSetting(
                $"ConnectionStrings:{PersistenceServiceCollectionExtensions.ConnectionStringName}",
                "Host=127.0.0.1;Port=1;Database=none;Username=none;Password=none;Timeout=2" );
        }
    }
}
