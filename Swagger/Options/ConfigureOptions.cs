namespace Core.Options
{
    using System;
    using System.Diagnostics.CodeAnalysis;
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

    /// <summary>Configuration settings for the <see cref="SwaggerGenOptions"/> and <see cref="SwaggerUIOptions"/> classes.</summary>
    [UsedImplicitly]
    [SuppressMessage("Performance", "CA1812:Avoid uninstantiated internal classes", Justification = "Used implicitly")]
    internal class ConfigureOptions : IConfigureOptions<SwaggerGenOptions>, IConfigureOptions<SwaggerUIOptions>
    {
        private readonly IApiVersionDescriptionProvider _apiVersionDescriptionProvider;
        private readonly SwaggerOptions _options;

        /// <summary>Initializes a new instance of the <see cref="ConfigureOptions"/> class.</summary>
        /// <param name="apiVersionDescriptionProvider">The API version description provider.</param>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="apiVersionDescriptionProvider"/> is <see langword="null" />
        /// or
        /// <paramref name="options"/> is <see langword="null" />.</exception>
        internal ConfigureOptions(
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
                _options.Info.Version = $"{apiVersionDescription.ApiVersion}";
                if (apiVersionDescription.IsDeprecated)
                {
                    var sb = new StringBuilder();
                    if (!IsNullOrWhiteSpace(_options.Info.Description))
                    {
                        sb.Append(_options.Info.Description);
                        sb.AppendLine();
                    }

                    sb.Append("This API version has been deprecated".ToUpperInvariant());
                    _options.Info.Description = sb.ToString();
                }

                options.SwaggerDoc(apiVersionDescription.GroupName, _options.Info);
            }

            if (!IsNullOrWhiteSpace(_options.DefaultScheme))
            {
                options.AddSecurityDefinition(_options.DefaultScheme, _options.SecurityScheme);
                options.OperationFilter<SecurityRequirementsOperationFilter>(_options.DefaultScheme);
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
            foreach (var apiVersionDescription in _apiVersionDescriptionProvider.ApiVersionDescriptions)
            {
                var name = apiVersionDescription.GroupName;
                var url = $"/swagger/{name}/swagger.json";
                options.SwaggerEndpoint(url, name);
            }

            if (!IsNullOrWhiteSpace(_options.RoutePrefix))
            {
                options.RoutePrefix = _options.RoutePrefix;
            }

            if (IsNullOrWhiteSpace(_options.Info?.Title))
            {
                return;
            }

            var sb = new StringBuilder(_options.Info?.Title);
            if (!IsNullOrWhiteSpace(_options.Info?.Description))
            {
                sb.AppendLine();
                sb.Append($"{_options.Info.Description}");
            }

            options.DocumentTitle = sb.ToString();
        }
    }
}
