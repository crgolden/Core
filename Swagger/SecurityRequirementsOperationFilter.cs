﻿namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(Operation? operation, OperationFilterContext? context)
        {
            if (operation == default)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context == default)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var controllerAttributes = context
                .MethodInfo?
                .DeclaringType?
                .GetCustomAttributes(true);
            var controllerAuthorizePolicyAttributes = controllerAttributes?
                .OfType<AuthorizeAttribute>()
                .Where(x => !string.IsNullOrEmpty(x.Policy))
                .Select(x => x.Policy);
            var controllerAuthorizeRoleAttributes = controllerAttributes?
                .OfType<AuthorizeAttribute>()
                .Where(x => !string.IsNullOrEmpty(x.Roles))
                .Select(x => x.Roles);
            var methodAttributes = context
                .MethodInfo?
                .CustomAttributes
                .ToArray();
            var methodAuthorizeAttributes = new List<string>();
            if (methodAttributes?.All(x => x.AttributeType != typeof(AllowAnonymousAttribute)) == true)
            {
                methodAuthorizeAttributes.AddRange(methodAttributes
                    .Where(x => x.AttributeType == typeof(AuthorizeAttribute))
                    .SelectMany(x => x.NamedArguments?
                        .Where(y => y.TypedValue.Value != null)
                        .Select(y => $"{y.TypedValue.Value}")));
            }

            var attributes = controllerAuthorizeRoleAttributes?
                .Union(controllerAuthorizePolicyAttributes)
                .Union(methodAuthorizeAttributes)
                .ToArray() ?? Array.Empty<string>();
            if (!attributes.Any())
            {
                return;
            }

            operation.Responses.Add("401", new Response { Description = "Unauthorized" });
            operation.Responses.Add("403", new Response { Description = "Forbidden" });
            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", attributes }
                }
            };
        }
    }
}
