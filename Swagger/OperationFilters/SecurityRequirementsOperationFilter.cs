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
    using static Microsoft.OpenApi.Models.ReferenceType;

    /// <inheritdoc />
    [PublicAPI]
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly OpenApiSecurityScheme _scheme;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityRequirementsOperationFilter"/> class.
        /// </summary>
        /// <param name="scheme">The default authentication scheme.</param>
        public SecurityRequirementsOperationFilter(OpenApiSecurityScheme scheme)
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

            operation.Responses = operation.Responses ?? new OpenApiResponses();
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

            operation.Security = operation.Security ?? new List<OpenApiSecurityRequirement>();
            if (operation.Security.Any(x => x.ContainsKey(_scheme)))
            {
                return;
            }

            var scopeNames = new List<string>();
            if (_scheme.Type == SecuritySchemeType.OAuth2 || _scheme.Type == SecuritySchemeType.OpenIdConnect)
            {
                _scheme.Flows = _scheme.Flows ?? new OpenApiOAuthFlows();
                if (_scheme.Flows.Implicit != default)
                {
                    scopeNames.AddRange(_scheme.Flows.Implicit.Scopes.Keys);
                }

                if (_scheme.Flows.AuthorizationCode != default)
                {
                    scopeNames.AddRange(_scheme.Flows.AuthorizationCode.Scopes.Keys);
                }

                if (_scheme.Flows.ClientCredentials != default)
                {
                    scopeNames.AddRange(_scheme.Flows.ClientCredentials.Scopes.Keys);
                }

                if (_scheme.Flows.Password != default)
                {
                    scopeNames.AddRange(_scheme.Flows.Password.Scopes.Keys);
                }
            }

            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = SecurityScheme,
                            Id = _scheme.Name
                        }
                    }

                ] = scopeNames
            });
        }
    }
}
