using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace RoadmapPlusPlus.Api.Persistence
{
    /// <summary>
    /// Настройки <c>dotnet ef</c>: EF ищет их в startup-проекте, то есть здесь.
    /// </summary>
    internal sealed class DesignTimeServices : IDesignTimeServices
    {
        public void ConfigureDesignTimeServices( IServiceCollection services )
        {
            services.Replace( ServiceDescriptor.Singleton<IMigrationsCodeGenerator, InternalMigrationsGenerator>() );
        }
    }
}
