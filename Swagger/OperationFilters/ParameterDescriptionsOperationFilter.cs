namespace Core.OperationFilters
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.OpenApi.Any;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using static System.String;

    /// <inheritdoc />
    [UsedImplicitly]
    internal class ParameterDescriptionsOperationFilter : IOperationFilter
    {
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

            operation.Deprecated |= context.ApiDescription.IsDeprecated();
            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters)
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                if (IsNullOrWhiteSpace(parameter.Description))
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Schema.Default == default && description.DefaultValue != default)
                {
                    parameter.Schema.Default = new OpenApiString(description.DefaultValue.ToString());
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}
