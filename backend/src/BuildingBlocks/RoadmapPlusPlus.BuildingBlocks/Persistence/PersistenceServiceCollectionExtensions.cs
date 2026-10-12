using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace RoadmapPlusPlus.BuildingBlocks.Persistence
{
    public static class PersistenceServiceCollectionExtensions
    {
        public const string ConnectionStringName = "Default";

        public const string MigrationsHistoryTable = "__ef_migrations_history";

        /// <summary>
        /// Регистрирует общий для всех модулей <see cref="NpgsqlDataSource"/> из <c>ConnectionStrings:Default</c>
        /// и проверку БД <c>postgres</c> для <c>/api/health</c>. Вызывает Bootstrapper.
        /// </summary>
        public static IServiceCollection AddPostgres( this IServiceCollection services, IConfiguration configuration )
        {
            services.AddSingleton( provider =>
            {
                string connectionString = configuration.GetConnectionString( ConnectionStringName )
                    ?? throw new InvalidOperationException( $"Не задана строка подключения ConnectionStrings:{ConnectionStringName}." );

                return new NpgsqlDataSourceBuilder( connectionString )
                    .UseLoggerFactory( provider.GetRequiredService<ILoggerFactory>() )
                    .Build();
            } );

            services.AddHealthChecks().AddCheck<PostgresHealthCheck>( "postgres", timeout: TimeSpan.FromSeconds( 5 ) );

            return services;
        }

        /// <summary>
        /// Регистрирует <see cref="DbContext"/> модуля поверх общего <see cref="NpgsqlDataSource"/>: имена в snake_case,
        /// история миграций — в схеме модуля. Миграции применяет <see cref="PersistenceServiceProviderExtensions.MigrateModuleDatabasesAsync"/>.
        /// </summary>
        public static IServiceCollection AddModuleDbContext<TContext>( this IServiceCollection services )
            where TContext : ModuleDbContext
        {
            string schema = ModuleSchema.Of( typeof( TContext ) );

            services.AddDbContext<TContext>( ( provider, options ) => options
                .UseNpgsql( provider.GetRequiredService<NpgsqlDataSource>(), npgsql => npgsql.MigrationsHistoryTable( MigrationsHistoryTable, schema ) )
                .UseSnakeCaseNamingConvention() );
            services.AddSingleton( new ModuleDbContextType( typeof( TContext ) ) );

            return services;
        }
    }
}
