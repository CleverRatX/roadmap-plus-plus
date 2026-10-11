using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RoadmapPlusPlus.BuildingBlocks;

namespace RoadmapPlusPlus.Modules.Planning
{
    public sealed class PlanningModule : IModule
    {
        public void AddServices( IServiceCollection services, IConfiguration configuration )
        {
        }

        public void MapEndpoints( IEndpointRouteBuilder endpoints )
        {
        }
    }
}
