namespace Core
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.Options;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwagger(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider,
            IOptions<SwaggerOptions> swaggerOptions,
            string routePrefix = "")
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    options.SwaggerEndpoint(
                        url: $"/swagger/{description.GroupName}/swagger.json",
                        name: description.GroupName.ToUpperInvariant());
                }

                options.RoutePrefix = routePrefix;
                options.DocumentTitle = $"{swaggerOptions.Value?.Title} {swaggerOptions.Value?.Description}";
            });
            return app;
        }
    }
}
