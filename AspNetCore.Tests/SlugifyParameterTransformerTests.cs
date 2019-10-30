namespace Core.AspNetCore.Tests
{
    using Xunit;

    public class SlugifyParameterTransformerTests
    {
        [Fact]
        public void TransformOutbound()
        {
            // Arrange
            const string route = "TestRoute";
            var slugifyParameterTransformer = new SlugifyParameterTransformer();

            // Act
            var response = slugifyParameterTransformer.TransformOutbound(route);

            // Assert
            Assert.Equal("test-route", response);
        }

        [Fact]
        public void TransformOutboundNullValue()
        {
            // Arrange
            var slugifyParameterTransformer = new SlugifyParameterTransformer();

            // Act
            var response = slugifyParameterTransformer.TransformOutbound(default);

            // Assert
            Assert.Equal(default, response);
        }
    }
}
