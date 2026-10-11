using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace RoadmapPlusPlus.BuildingBlocks
{
    /// <summary>
    /// Точка подключения модуля к приложению. Bootstrapper вызывает её у каждого модуля при старте.
    /// </summary>
    public interface IModule
    {
        /// <summary>Регистрирует сервисы модуля в DI.</summary>
        void AddServices( IServiceCollection services, IConfiguration configuration );

        /// <summary>Добавляет эндпоинты модуля. <paramref name="endpoints"/> уже под префиксом <c>/api</c>.</summary>
        void MapEndpoints( IEndpointRouteBuilder endpoints );
    }
}
