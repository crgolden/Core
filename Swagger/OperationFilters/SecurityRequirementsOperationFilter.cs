namespace Core.OperationFilters
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using static System.Net.HttpStatusCode;
    using static System.String;
    using static Microsoft.OpenApi.Models.ReferenceType;

    /// <inheritdoc />
    [UsedImplicitly]
    internal class SecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly string _scheme;

        public SecurityRequirementsOperationFilter(string scheme = "Bearer")
        {
            _scheme = scheme;
        }

        /// <inheritdoc />
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation == default)
            {
                throw new ArgumentNullException(nameof(operation));
            }

            if (context == default)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var controllerAttributes = new List<AuthorizeAttribute>();
            if (context.MethodInfo.DeclaringType != null)
            {
                controllerAttributes.AddRange(context
                    .MethodInfo
                    .DeclaringType
                    .GetTypeInfo()
                    .GetCustomAttributes<AuthorizeAttribute>());
            }

            var methodAttributes = context.MethodInfo.GetCustomAttributes<AuthorizeAttribute>().ToArray();
            if (!controllerAttributes.Any() && !methodAttributes.Any())
            {
                return;
            }

            var attributes = controllerAttributes
                .Select(x => x.Policy)
                .Union(controllerAttributes.Select(y => y.Roles))
                .Union(methodAttributes.Select(y => y.Policy))
                .Union(methodAttributes.Select(y => y.Roles))
                .Where(x => !IsNullOrWhiteSpace(x))
                .Distinct()
                .ToArray();
            operation.Responses ??= new OpenApiResponses();
            var key = $"{(int)Unauthorized}";
            if (!operation.Responses.ContainsKey(key))
            {
                operation.Responses.Add(key, new OpenApiResponse
                {
                    Description = $"{Unauthorized}"
                });
            }

            key = $"{(int)Forbidden}";
            if (!operation.Responses.ContainsKey(key))
            {
                operation.Responses.Add(key, new OpenApiResponse
                {
                    Description = $"{Forbidden}"
                });
            }

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            var scheme = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = SecurityScheme,
                    Id = _scheme
                }
            };
            if (!operation.Security.Any(x => x.ContainsKey(scheme)))
            {
                operation.Security.Add(new OpenApiSecurityRequirement
                {
                    [scheme] = attributes
                });
            }
        }
    }
}
