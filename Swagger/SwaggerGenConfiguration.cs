namespace Core
{
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class SwaggerGenConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
        private readonly string _defaultScheme;
        private readonly ApiKeyScheme _apiKeyScheme;
        private readonly Info _info;

        public SwaggerGenConfiguration(
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            IOptions<SwaggerOptions> swaggerOptions)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider;
            _defaultScheme = swaggerOptions.Value?.DefaultScheme;
            _apiKeyScheme = new ApiKeyScheme
            {
                Name = swaggerOptions.Value?.ApiKeySchemeName,
                Description = swaggerOptions.Value?.ApiKeySchemeDescription,
                In = swaggerOptions.Value?.ApiKeySchemeIn,
                Type = swaggerOptions.Value?.ApiKeySchemeType
            };
            _info = new Info
            {
                Title = swaggerOptions.Value?.Title,
                Description = swaggerOptions.Value?.Description,
                TermsOfService = swaggerOptions.Value?.TermsOfService,
                Contact = new Contact
                {
                    Name = swaggerOptions.Value?.ContactName,
                    Email = swaggerOptions.Value?.ContactEmail,
                    Url = swaggerOptions.Value?.ContactUrl
                },
                License = new License
                {
                    Name = swaggerOptions.Value?.LicenseName,
                    Url = swaggerOptions.Value?.LicenseUrl
                }
            };
        }

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                _info.Version = $"{description.ApiVersion}";
                if (description.IsDeprecated) _info.Description += " This API version has been deprecated.";
                options.SwaggerDoc(description.GroupName, _info);
                options.AddSecurityDefinition(_defaultScheme, _apiKeyScheme);
            }
        }
    }
}
