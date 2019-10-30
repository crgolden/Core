namespace Core
{
    using System;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using static System.String;

    public static class ServiceCollectionExtensions
    {
        // format the version as "'v'major[.minor][-status]"
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            IConfiguration? configuration,
            string? xmlCommentsFilePath = default)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection(nameof(SwaggerOptions));
            if (!section.Exists())
            {
                throw new ArgumentException(
                    message: $"{nameof(SwaggerOptions)} section doesn't exist",
                    paramName: nameof(configuration));
            }

            services.Configure<SwaggerOptions>(section);
            var options = section.Get<SwaggerOptions>();
            if (options == default ||
                IsNullOrEmpty(options.Title) ||
                IsNullOrEmpty(options.DefaultScheme))
            {
                throw new ArgumentException(
                    message: $"{nameof(SwaggerOptions)} section is invalid",
                    paramName: nameof(configuration));
            }

            services.AddApiVersioning(setupAction =>
            {
                setupAction.AssumeDefaultVersionWhenUnspecified = options.AssumeDefaultVersionWhenUnspecified;
                setupAction.ReportApiVersions = options.ReportApiVersions;
            });
            services.AddVersionedApiExplorer(setupAction =>
            {
                setupAction.GroupNameFormat = options.GroupNameFormat;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerGenConfiguration>();
            services.AddSwaggerGen(setupAction =>
            {
                if (IsNullOrEmpty(xmlCommentsFilePath))
                {
                    return;
                }

                setupAction.IncludeXmlComments(xmlCommentsFilePath);
            });
            return services;
        }
    }
}
