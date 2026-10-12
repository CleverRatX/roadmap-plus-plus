using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace RoadmapPlusPlus.BuildingBlocks.Persistence
{
    public static class PersistenceServiceProviderExtensions
    {
        /// <summary>
        /// Применяет миграции всех <see cref="DbContext"/>, зарегистрированных через
        /// <see cref="PersistenceServiceCollectionExtensions.AddModuleDbContext{TContext}"/>. Bootstrapper вызывает его при старте.
        /// </summary>
        public static async Task MigrateModuleDatabasesAsync( this IServiceProvider services, CancellationToken cancellationToken = default )
        {
            await using AsyncServiceScope scope = services.CreateAsyncScope();

            foreach ( ModuleDbContextType context in scope.ServiceProvider.GetServices<ModuleDbContextType>() )
            {
                DbContext dbContext = ( DbContext )scope.ServiceProvider.GetRequiredService( context.Type );
                await dbContext.Database.MigrateAsync( cancellationToken );
            }
        }
    }
}
