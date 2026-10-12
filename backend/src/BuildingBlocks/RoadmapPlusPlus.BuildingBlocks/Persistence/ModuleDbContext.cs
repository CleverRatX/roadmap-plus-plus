using Microsoft.EntityFrameworkCore;

namespace RoadmapPlusPlus.BuildingBlocks.Persistence
{
    /// <summary>
    /// Базовый <see cref="DbContext"/> модуля. Таблицы лежат в схеме модуля — его имени в нижнем регистре
    /// (<c>RoadmapPlusPlus.Modules.Planning</c> → <c>planning</c>), конфигурации сущностей берутся из сборки модуля.
    /// Регистрируется через <see cref="PersistenceServiceCollectionExtensions.AddModuleDbContext{TContext}"/>.
    /// </summary>
    public abstract class ModuleDbContext( DbContextOptions options ) : DbContext( options )
    {
        protected override void OnModelCreating( ModelBuilder modelBuilder )
        {
            modelBuilder.HasDefaultSchema( ModuleSchema.Of( GetType() ) );
            modelBuilder.ApplyConfigurationsFromAssembly( GetType().Assembly );
        }
    }
}
