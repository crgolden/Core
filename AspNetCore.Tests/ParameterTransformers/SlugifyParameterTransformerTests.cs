namespace Core.AspNetCore.Tests.ParameterTransformers
{
    using System;
    using Core.ParameterTransformers;
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
        public void TransformOutboundThrowsForNullValue()
        {
            // Arrange
            string TestCode() => new SlugifyParameterTransformer().TransformOutbound(default);

            // Act
            var exception = Assert.Throws<ArgumentNullException>(TestCode);

            // Assert
            Assert.Equal("value", exception.ParamName);
        }
    }
}
