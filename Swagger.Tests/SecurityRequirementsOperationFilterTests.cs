namespace Core.Swagger.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Xunit;
    using static System.Net.HttpStatusCode;

    public class SecurityRequirementsOperationFilterTests
    {
        private readonly Operation _operation;

        public SecurityRequirementsOperationFilterTests()
        {
            _operation = new Operation
            {
                OperationId = $"{Guid.NewGuid()}",
                Responses = new Dictionary<string, Response>()
            };
        }

        [Fact]
        public void ApplyMethodRoles()
        {
            // Arrange
            var context = GetContext(typeof(Controller), nameof(Controller.MethodWithRoles));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            Assert.NotNull(attributes);
            var role = Assert.Single(attributes["Bearer"]);
            Assert.Equal("AdminRole", role);
        }

        [Fact]
        public void ApplyMethodPolicies()
        {
            // Arrange
            var context = GetContext(typeof(Controller), nameof(Controller.MethodWithPolicies));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            Assert.NotNull(attributes);
            var policy = Assert.Single(attributes["Bearer"]);
            Assert.Equal("AdminPolicy", policy);
        }

        [Fact]
        public void ApplyMethodAllowAnonymousAttribute()
        {
            // Arrange
            var context = GetContext(typeof(Controller), nameof(Controller.MethodWithAllowAnonymous));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            Assert.Empty(_operation.Responses);
            Assert.Null(_operation.Security);
        }

        [Fact]
        public void ApplyControllerRoles()
        {
            // Arrange
            var context = GetContext(typeof(ControllerWithRoles), nameof(ControllerWithRoles.Method));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            Assert.NotNull(attributes);
            var policy = Assert.Single(attributes["Bearer"]);
            Assert.Equal("UserRole", policy);
        }

        [Fact]
        public void ApplyControllerPolicies()
        {
            // Arrange
            var context = GetContext(typeof(ControllerWithPolicies), nameof(ControllerWithPolicies.Method));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            AssertAuthorizeResponses();
            var security = Assert.IsAssignableFrom<List<IDictionary<string, IEnumerable<string>>>>(_operation.Security);
            var attributes = Assert.Single(security);
            Assert.NotNull(attributes);
            var policy = Assert.Single(attributes["Bearer"]);
            Assert.Equal("UserPolicy", policy);
        }

        [Fact]
        public void ApplyControllerAllowAnonymousAttribute()
        {
            // Arrange
            var context = GetContext(typeof(ControllerWithAllowAnonymous), nameof(ControllerWithAllowAnonymous.Method));
            var filter = new SecurityRequirementsOperationFilter();

            // Act
            filter.Apply(_operation, context);

            // Assert
            Assert.Empty(_operation.Responses);
            Assert.Null(_operation.Security);
        }

        [Fact]
        public void ApplyThrowsForNullOperation()
        {
            // Arrange
            var context = new OperationFilterContext(default, default, default);
            var filter = new SecurityRequirementsOperationFilter();
            void TestCode() => filter.Apply(default, context);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("operation", exception.ParamName);
        }

        [Fact]
        public void ApplyThrowsForNullContext()
        {
            // Arrange
            var filter = new SecurityRequirementsOperationFilter();
            void TestCode() => filter.Apply(_operation, default);

            // Act / Assert
            var exception = Assert.Throws<ArgumentNullException>(TestCode);
            Assert.Equal("context", exception.ParamName);
        }

        private static OperationFilterContext GetContext(IReflect controllerType, string methodName)
        {
            var methodInfo = controllerType.GetMethod(
                name: methodName,
                bindingAttr: BindingFlags.NonPublic | BindingFlags.Static);
            return new OperationFilterContext(
                apiDescription: new ApiDescription(),
                schemaRegistry: default,
                methodInfo: methodInfo);
        }

        private void AssertAuthorizeResponses()
        {
            Assert.Collection(
                _operation.Responses,
                response1 =>
                {
                    var (key, value) = response1;
                    Assert.Equal($"{(int)Unauthorized}", key);
                    var response = Assert.IsType<Response>(value);
                    Assert.Equal($"{Unauthorized}", response.Description);
                },
                response2 =>
                {
                    var (key, value) = response2;
                    Assert.Equal($"{(int)Forbidden}", key);
                    var response = Assert.IsType<Response>(value);
                    Assert.Equal($"{Forbidden}", response.Description);
                });
        }

        private static class Controller
        {
            [Authorize(Roles = "AdminRole")]
            internal static void MethodWithRoles()
            {
            }

            [Authorize(Policy = "AdminPolicy")]
            internal static void MethodWithPolicies()
            {
            }

            [AllowAnonymous]
            internal static void MethodWithAllowAnonymous()
            {
            }
        }

        [Authorize(Roles = "UserRole")]
        private static class ControllerWithRoles
        {
            internal static void Method()
            {
            }
        }

        [Authorize(Policy = "UserPolicy")]
        private static class ControllerWithPolicies
        {
            internal static void Method()
            {
            }
        }

        [AllowAnonymous]
        private static class ControllerWithAllowAnonymous
        {
            internal static void Method()
            {
            }
        }
    }
}
