using System.Net;
using System.Text.Json;

namespace RoadmapPlusPlus.IntegrationTests
{
    public sealed class ErrorHandlingTests( PostgresApiFactory api )
    {
        [Fact]
        public async Task UnknownRouteIsProblemWithRequestId()
        {
            using HttpClient client = api.CreateClient();

            using HttpResponseMessage response = await client.GetAsync( "/api/does-not-exist", TestContext.Current.CancellationToken );

            Assert.Equal( HttpStatusCode.NotFound, response.StatusCode );
            Assert.Equal( "application/problem+json", response.Content.Headers.ContentType?.MediaType );

            string requestId = Assert.Single( response.Headers.GetValues( "X-Request-Id" ) );
            using JsonDocument body = JsonDocument.Parse( await response.Content.ReadAsStringAsync( TestContext.Current.CancellationToken ) );
            Assert.Equal( requestId, body.RootElement.GetProperty( "requestId" ).GetString() );
            Assert.False( body.RootElement.TryGetProperty( "traceId", out _ ) );
        }

        [Fact]
        public async Task SuccessfulResponseHasRequestId()
        {
            using HttpClient client = api.CreateClient();

            using HttpResponseMessage response = await client.GetAsync( "/api/health", TestContext.Current.CancellationToken );

            Assert.False( string.IsNullOrEmpty( Assert.Single( response.Headers.GetValues( "X-Request-Id" ) ) ) );
        }
    }
}
