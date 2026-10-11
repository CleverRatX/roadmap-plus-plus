using RoadmapPlusPlus.BuildingBlocks;
using RoadmapPlusPlus.Modules.Assistant;
using RoadmapPlusPlus.Modules.Identity;
using RoadmapPlusPlus.Modules.Interchange;
using RoadmapPlusPlus.Modules.Knowledge;
using RoadmapPlusPlus.Modules.Planning;

IModule[] modules =
[
    new IdentityModule(),
    new PlanningModule(),
    new KnowledgeModule(),
    new InterchangeModule(),
    new AssistantModule(),
];

WebApplicationBuilder builder = WebApplication.CreateBuilder( args );

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddHealthChecks();

foreach ( IModule module in modules )
{
    module.AddServices( builder.Services, builder.Configuration );
}

WebApplication app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

RouteGroupBuilder api = app.MapGroup( "/api" );
api.MapOpenApi( "/openapi/{documentName}.json" );
api.MapHealthChecks( "/health" );

foreach ( IModule module in modules )
{
    module.MapEndpoints( api );
}

app.Run();
