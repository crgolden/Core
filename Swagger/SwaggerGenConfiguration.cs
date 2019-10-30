namespace Core
{
    using System;
    using System.Text;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    /// <inheritdoc />
    public class SwaggerGenConfiguration : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
        private readonly string _defaultScheme;
        private readonly ApiKeyScheme _apiKeyScheme;
        private readonly Info _info;

        public SwaggerGenConfiguration(
            IApiVersionDescriptionProvider? apiVersionDescriptionProvider,
            IOptions<SwaggerOptions>? swaggerOptions)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider ?? throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));
            if (swaggerOptions?.Value == default)
            {
                throw new ArgumentNullException(nameof(swaggerOptions));
            }

            _defaultScheme = swaggerOptions.Value.DefaultScheme;
            _apiKeyScheme = new ApiKeyScheme
            {
                Name = swaggerOptions.Value.ApiKeySchemeName,
                Description = swaggerOptions.Value.ApiKeySchemeDescription,
                In = swaggerOptions.Value.ApiKeySchemeIn,
                Type = swaggerOptions.Value.ApiKeySchemeType
            };
            _info = new Info
            {
                Title = swaggerOptions.Value.Title,
                Description = swaggerOptions.Value.Description,
                TermsOfService = swaggerOptions.Value.TermsOfService,
                Contact = new Contact
                {
                    Name = swaggerOptions.Value.ContactName,
                    Email = swaggerOptions.Value.ContactEmail,
                    Url = $"{swaggerOptions.Value.ContactUrl}"
                },
                License = new License
                {
                    Name = swaggerOptions.Value.LicenseName,
                    Url = $"{swaggerOptions.Value.LicenseUrl}"
                }
            };
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions? options)
        {
            if (options == default)
            {
                throw new ArgumentNullException(nameof(options));
            }

            foreach (var apiVersionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                _info.Version = $"{apiVersionDescription.ApiVersion}";
                var sb = new StringBuilder(_info.Description);
                if (apiVersionDescription.IsDeprecated)
                {
                    sb.Append(" This API version has been deprecated.");
                }

                _info.Description = $"{sb}";
                options.SwaggerDoc(apiVersionDescription.GroupName, _info);
                options.AddSecurityDefinition(_defaultScheme, _apiKeyScheme);
            }

            options.OperationFilter<ParameterDescriptionsOperationFilter>();
            options.OperationFilter<SecurityRequirementsOperationFilter>();
        }
    }
}
