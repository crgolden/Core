namespace Core
{
    using System;
    using System.Text;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Mvc.ApiExplorer;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Microsoft.OpenApi.Models;
    using OperationFilters;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using static System.String;

    /// <inheritdoc cref="IConfigureOptions{TOptions}" />
    [PublicAPI]
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>, IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
        private readonly SwaggerOptions _options;

        /// <summary>Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.</summary>
        /// <param name="apiVersionDescriptionProvider">The API version description provider.</param>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="apiVersionDescriptionProvider"/> is <see langword="null" />
        /// or
        /// <paramref name="options"/> is <see langword="null" />.</exception>
        public ConfigureSwaggerOptions(
            IApiVersionDescriptionProvider apiVersionDescriptionProvider,
            IOptions<SwaggerOptions> options)
        {
            _apiVersionDescriptionProvider = apiVersionDescriptionProvider ?? throw new ArgumentNullException(nameof(apiVersionDescriptionProvider));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            _options.Info = _options.Info ?? new OpenApiInfo();
            foreach (var apiVersionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                var sb = new StringBuilder();
                if (!IsNullOrWhiteSpace(_options.Info.Description))
                {
                    sb.Append(_options.Info.Description);
                }

                if (apiVersionDescription.IsDeprecated)
                {
                    if (sb.Length > 0)
                    {
                        sb.AppendLine();
                    }

                    sb.Append("This API version has been deprecated".ToUpperInvariant());
                }

                if (sb.Length > 0)
                {
                    _options.Info.Description = sb.ToString();
                }

                _options.Info.Version = $"{apiVersionDescription.ApiVersion}";
                options.SwaggerDoc(apiVersionDescription.GroupName, _options.Info);
            }

            if (_options.SecurityScheme != default)
            {
                options.AddSecurityDefinition(_options.SecurityScheme.Name, _options.SecurityScheme);
                options.OperationFilter<SecurityRequirementsOperationFilter>(_options.SecurityScheme);
            }

            if (!IsNullOrWhiteSpace(_options.XmlCommentsFilePath))
            {
                options.IncludeXmlComments(_options.XmlCommentsFilePath);
            }

            options.OperationFilter<ParameterDescriptionsOperationFilter>();
            options.EnableAnnotations();
        }

        /// <inheritdoc />
        public void Configure(SwaggerUIOptions options)
        {
            if (options == default)
            {
                throw new ArgumentNullException(nameof(options));
            }

            foreach (var apiVersionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                var name = apiVersionDescription.GroupName;
                var url = $"./swagger/{name}/swagger.json";
                options.SwaggerEndpoint(url, name);
            }

            options.RoutePrefix = _options.RoutePrefix;
            if (IsNullOrWhiteSpace(_options.Info?.Title))
            {
                return;
            }

            var sb = new StringBuilder(_options.Info.Title);
            if (!IsNullOrWhiteSpace(_options.Info?.Description))
            {
                sb.AppendLine();
                sb.Append(_options.Info.Description);
            }

            options.DocumentTitle = sb.ToString();
            if (_options.OAuthConfig != default)
            {
                options.OAuthConfigObject = _options.OAuthConfig;
            }
        }
    }
}
