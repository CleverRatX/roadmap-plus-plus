namespace RoadmapPlusPlus.BuildingBlocks.Tests
{
    public sealed class ResultTests
    {
        private static readonly Error NotFound = Error.NotFound( "test.not_found", "Не найдено." );

        [Fact]
        public void SuccessHasNoError()
        {
            Result result = Result.Success();

            Assert.True( result.IsSuccess );
            Assert.Null( result.Error );
        }

        [Fact]
        public void ErrorConvertsToFailedResult()
        {
            Result result = NotFound;

            Assert.False( result.IsSuccess );
            Assert.Same( NotFound, result.Error );
        }

        [Fact]
        public void ValueConvertsToSuccessfulResult()
        {
            Result<int> result = 42;

            Assert.True( result.IsSuccess );
            Assert.Equal( 42, result.Value );
        }

        [Fact]
        public void ErrorConvertsToFailedResultOfT()
        {
            Result<int> result = NotFound;

            Assert.False( result.IsSuccess );
            Assert.Same( NotFound, result.Error );
        }

        [Fact]
        public void ValueOfFailedResultThrows()
        {
            Result<int> result = NotFound;

            InvalidOperationException exception = Assert.Throws<InvalidOperationException>( () => result.Value );
            Assert.Contains( "test.not_found", exception.Message, StringComparison.Ordinal );
        }

        [Fact]
        public void NullValueIsSuccess()
        {
            Result<string?> result = ( string? )null;

            Assert.True( result.IsSuccess );
            Assert.Null( result.Value );
        }
    }
}
