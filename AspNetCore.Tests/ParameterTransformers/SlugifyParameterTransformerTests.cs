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
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.
            static string TestCode() => new SlugifyParameterTransformer().TransformOutbound(default);
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            // Act
            var exception = Assert.Throws<ArgumentNullException>(TestCode);

            // Assert
            Assert.Equal("value", exception.ParamName);
        }
    }
}
