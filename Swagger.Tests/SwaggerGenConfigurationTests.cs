namespace Core.Swagger.Tests
{
    using System;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class SwaggerGenConfigurationTests
    {
        [Fact]
        public void ThrowsForNullApiVersionDescriptionProvider()
        {
            // Arrange
            var swaggerOptions = new Mock<IOptions<SwaggerOptions>>();
            swaggerOptions.Setup(x => x.Value).Returns(new SwaggerOptions());
            object TestCode() => new SwaggerGenConfiguration(default, swaggerOptions.Object);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("apiVersionDescriptionProvider", exception.ParamName);
        }

        [Fact]
        public void ThrowsForNullSwaggerOptions()
        {
            // Arrange
            var apiVersionDescriptionProvider = Mock.Of<IApiVersionDescriptionProvider>();
            object TestCode() => new SwaggerGenConfiguration(apiVersionDescriptionProvider, default);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("swaggerOptions", exception.ParamName);
        }

        [Fact]
        public void ConfigureThrowsForNullSwaggerGenOptions()
        {
            // Arrange
            var apiVersionDescriptionProvider = Mock.Of<IApiVersionDescriptionProvider>();
            var swaggerOptions = new Mock<IOptions<SwaggerOptions>>();
            swaggerOptions.Setup(x => x.Value).Returns(new SwaggerOptions());
            var swaggerGenConfiguration = new SwaggerGenConfiguration(apiVersionDescriptionProvider, swaggerOptions.Object);
            void TestCode() => swaggerGenConfiguration.Configure(default);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("options", exception.ParamName);
        }
    }
}
