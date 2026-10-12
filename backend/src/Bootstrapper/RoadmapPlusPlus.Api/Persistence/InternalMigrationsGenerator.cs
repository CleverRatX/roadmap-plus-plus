using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace RoadmapPlusPlus.Api.Persistence
{
    /// <summary>
    /// Генерирует класс миграции <c>internal</c>, а не <c>public</c>: модуль наружу показывает только свой <c>IModule</c>.
    /// Снапшот модели EF и так создаёт без модификатора доступа.
    /// </summary>
    internal sealed class InternalMigrationsGenerator(
        MigrationsCodeGeneratorDependencies dependencies,
        CSharpMigrationsGeneratorDependencies csharpDependencies )
        : CSharpMigrationsGenerator( dependencies, csharpDependencies )
    {
        public override string GenerateMigration(
            string? migrationNamespace,
            string migrationName,
            IReadOnlyList<MigrationOperation> upOperations,
            IReadOnlyList<MigrationOperation> downOperations )
        {
            return base.GenerateMigration( migrationNamespace, migrationName, upOperations, downOperations )
                .Replace( "public partial class ", "internal partial class ", StringComparison.Ordinal );
        }
    }
}
