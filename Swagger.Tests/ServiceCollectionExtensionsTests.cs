namespace Core.Swagger.Tests
{
    using System;
    using Microsoft.Extensions.DependencyInjection;
    using Xunit;

    public class ServiceCollectionExtensionsTests
    {
        [Fact]
        public void AddSwaggerThrowsForNullConfiguration()
        {
            // Arrange
            var services = new ServiceCollection();
            object TestCode() => services.AddSwagger(default);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("configuration", exception.ParamName);
        }
    }
}
