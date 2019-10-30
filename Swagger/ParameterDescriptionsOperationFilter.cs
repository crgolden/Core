namespace Core
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using static System.String;

    /// <inheritdoc />
    public class ParameterDescriptionsOperationFilter : IOperationFilter
    {
        /// <inheritdoc />
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

            operation.Deprecated |= context.ApiDescription.IsDeprecated();
            if (operation.Parameters == null)
            {
                return;
            }

            foreach (var parameter in operation.Parameters.OfType<NonBodyParameter>())
            {
                var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
                if (IsNullOrEmpty(parameter.Description))
                {
                    parameter.Description = description.ModelMetadata?.Description;
                }

                if (parameter.Default == null)
                {
                    parameter.Default = description.DefaultValue;
                }

                parameter.Required |= description.IsRequired;
            }
        }
    }
}
