namespace Core.Swagger.Tests
{
    using System;
    using System.Collections.Generic;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;

    public class ParameterDescriptionsOperationFilterTests
    {
        private readonly Operation _operation;

        public ParameterDescriptionsOperationFilterTests()
        {
            _operation = new Operation
            {
                OperationId = $"{Guid.NewGuid()}",
                Responses = new Dictionary<string, Response>()
            };
        }

        [Fact]
        public void ApplyThrowsForNullOperation()
        {
            // Arrange
            var context = new OperationFilterContext(default, default, default);
            var filter = new ParameterDescriptionsOperationFilter();
            void TestCode() => filter.Apply(default, context);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("operation", exception.ParamName);
        }

        [Fact]
        public void ApplyThrowsForNullContext()
        {
            // Arrange
            var filter = new ParameterDescriptionsOperationFilter();
            void TestCode() => filter.Apply(_operation, default);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("context", exception.ParamName);
        }
    }
}
