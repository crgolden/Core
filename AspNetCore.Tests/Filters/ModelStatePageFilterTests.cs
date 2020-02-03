namespace Core.AspNetCore.Tests.Filters
{
    using System;
    using System.Collections.Generic;
    using Core.Filters;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.Mvc.RazorPages.Infrastructure;
    using Microsoft.AspNetCore.Routing;
    using Xunit;

    public class ModelStatePageFilterTests
    {
        private readonly ModelStatePageFilter _filter;
        private readonly PageContext _context;

        public ModelStatePageFilterTests()
        {
            _filter = new ModelStatePageFilter();
            _context = new PageContext(new ActionContext(
                httpContext: new DefaultHttpContext(),
                routeData: new RouteData(),
                actionDescriptor: new PageActionDescriptor(),
                modelState: new ModelStateDictionary()));
        }

        [Fact]
        public void OnPageHandlerSelected()
        {
            // Arrange
            var pageHandlerSelectedContext = new PageHandlerSelectedContext(
                pageContext: _context,
                filters: new List<IFilterMetadata>(),
                handlerInstance: new { });

            // Act
            _filter.OnPageHandlerSelected(pageHandlerSelectedContext);

            // Assert
            Assert.True(pageHandlerSelectedContext.ModelState.IsValid);
        }

        [Fact]
        public void OnPageHandlerExecutingValid()
        {
            // Arrange
            var pageHandlerExecutingContext = new PageHandlerExecutingContext(
                pageContext: _context,
                filters: new List<IFilterMetadata>(),
                handlerMethod: new HandlerMethodDescriptor(),
                handlerArguments: new Dictionary<string, object>(),
                handlerInstance: new { });

            // Act
            _filter.OnPageHandlerExecuting(pageHandlerExecutingContext);

            // Assert
            Assert.True(pageHandlerExecutingContext.ModelState.IsValid);
            Assert.Null(pageHandlerExecutingContext.Result);
        }

        [Fact]
        public void OnPageHandlerExecutingInvalid()
        {
            // Arrange
            const string errorKey = "Error Key";
            const string errorMessage = "Error Message";

            _context.ModelState.AddModelError(errorKey, errorMessage);

            var pageHandlerExecutingContext = new PageHandlerExecutingContext(
                pageContext: _context,
                filters: new List<IFilterMetadata>(),
                handlerMethod: new HandlerMethodDescriptor(),
                handlerArguments: new Dictionary<string, object>(),
                handlerInstance: new { });

            // Act
            _filter.OnPageHandlerExecuting(pageHandlerExecutingContext);

            // Assert
            Assert.False(pageHandlerExecutingContext.ModelState.IsValid);
            var result = Assert.IsType<BadRequestObjectResult>(pageHandlerExecutingContext.Result);
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
        public void OnPageHandlerExecutingNullContext()
        {
            // Arrange
            void TestCode() => _filter.OnPageHandlerExecuting(default);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("context", exception.ParamName);
        }

        [Fact]
        public void OnPageHandlerExecuted()
        {
            // Arrange
            var pageHandlerExecutedContext = new PageHandlerExecutedContext(
                pageContext: _context,
                filters: new List<IFilterMetadata>(),
                handlerMethod: new HandlerMethodDescriptor(),
                handlerInstance: new { });

            // Act
            _filter.OnPageHandlerExecuted(pageHandlerExecutedContext);

            // Assert
            Assert.True(pageHandlerExecutedContext.ModelState.IsValid);
        }
    }
}
