namespace Core.Swagger.Tests
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Options;
    using Moq;
    using Xunit;

    public class ApplicationBuilderExtensionsTests
    {
        [Fact]
        public void UseSwaggerThrowsForNullApiVersionDescriptionProvider()
        {
            // Arrange
            var swaggerOptions = Mock.Of<IOptions<SwaggerOptions>>();
            var app = Mock.Of<IApplicationBuilder>();
            object TestCode() => app.UseSwagger(default, swaggerOptions);

            // Act Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("apiVersionDescriptionProvider", exception.ParamName);
        }

        [Fact]
        public void UseSwaggerThrowsForNullSwaggerOptions()
        {
            // Arrange
            var apiVersionDescriptionProvider = Mock.Of<IApiVersionDescriptionProvider>();
            var app = Mock.Of<IApplicationBuilder>();
            object TestCode() => app.UseSwagger(apiVersionDescriptionProvider, default);

            // Act Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("swaggerOptions", exception.ParamName);
        }
    }
}
