using System.Net;

namespace RoadmapPlusPlus.IntegrationTests
{
    public sealed class HealthTests( PostgresApiFactory api )
    {
        [Fact]
        public async Task HealthyWithDatabase()
        {
            using HttpClient client = api.CreateClient();

            using HttpResponseMessage response = await client.GetAsync( "/api/health", TestContext.Current.CancellationToken );

            Assert.Equal( HttpStatusCode.OK, response.StatusCode );
            Assert.Equal( "Healthy", await response.Content.ReadAsStringAsync( TestContext.Current.CancellationToken ) );
        }

        [Fact]
        public async Task UnhealthyWithoutDatabase()
        {
            await using UnreachableDatabaseApiFactory unreachable = new();
            using HttpClient client = unreachable.CreateClient();

            using HttpResponseMessage response = await client.GetAsync( "/api/health", TestContext.Current.CancellationToken );

            Assert.Equal( HttpStatusCode.ServiceUnavailable, response.StatusCode );
            Assert.Equal( "Unhealthy", await response.Content.ReadAsStringAsync( TestContext.Current.CancellationToken ) );
        }
    }
}
