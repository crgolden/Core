namespace Core.AspNetCore.Tests
{
    using System;
    using System.Collections.Generic;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Abstractions;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Routing;
    using Xunit;

    public class ModelStateActionFilterTests
    {
        private readonly ModelStateActionFilter _filter;
        private readonly ActionContext _context;

        public ModelStateActionFilterTests()
        {
            _filter = new ModelStateActionFilter();
            _context = new ActionContext(
                httpContext: new DefaultHttpContext(),
                routeData: new RouteData(),
                actionDescriptor: new ActionDescriptor(),
                modelState: new ModelStateDictionary());
        }

        [Fact]
        public void OnActionExecutingValid()
        {
            // Arrange
            var actionExecutingContext = new ActionExecutingContext(
                actionContext: _context,
                filters: new List<IFilterMetadata>(),
                actionArguments: new Dictionary<string, object>(),
                controller: default);

            // Act
            _filter.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Null(actionExecutingContext.Result);
        }

        [Fact]
        public void OnActionExecutingInvalid()
        {
            // Arrange
            const string errorKey = "Error Key";
            const string errorMessage = "Error Message";

            _context.ModelState.AddModelError(errorKey, errorMessage);

            var actionExecutingContext = new ActionExecutingContext(
                actionContext: _context,
                filters: new List<IFilterMetadata>(),
                actionArguments: new Dictionary<string, object>(),
                controller: default);

            // Act
            _filter.OnActionExecuting(actionExecutingContext);

            // Assert
            var result = Assert.IsType<BadRequestObjectResult>(actionExecutingContext.Result);
            var error = Assert.IsType<SerializableError>(result.Value);
            Assert.Collection(error, collection =>
            {
                var (key, value) = collection;
                var messages = Assert.IsType<string[]>(value);
                Assert.Single(messages);
                Assert.Equal(errorKey, key);
                Assert.Equal(errorMessage, messages[0]);
            });
        }

        [Fact]
        public void OnActionExecutingNullContext()
        {
            // Arrange
            void TestCode() => _filter.OnActionExecuting(default);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("context", exception.ParamName);
        }
    }
}
