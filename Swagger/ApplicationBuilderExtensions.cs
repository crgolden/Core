namespace Core
{
    using System;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Options;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider? apiVersionDescriptionProvider,
            IOptions<SwaggerOptions>? swaggerOptions,
            string routePrefix = "")
        {
            if (apiVersionDescriptionProvider == default)
            {
                throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));
            }

            if (swaggerOptions?.Value == default)
            {
                throw new ArgumentNullException(nameof(swaggerOptions));
            }

            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var apiVersionDescription in apiVersionDescriptionProvider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        url: $"/swagger/{apiVersionDescription.GroupName}/swagger.json",
                        name: apiVersionDescription.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = routePrefix;
                options.DocumentTitle = $"{swaggerOptions.Value.Title} {swaggerOptions.Value.Description}";
            });
            return app;
        }
    }
}
