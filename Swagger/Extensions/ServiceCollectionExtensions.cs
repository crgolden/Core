namespace Core.Extensions
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNet.OData.Extensions;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Options;
    using Options;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;

    /// <summary>A class with methods that extend <see cref="IServiceCollection"/>.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds <see cref="SwaggerOptions"/> and <see cref="ConfigureOptions"/> instances to <paramref name="services"/>.</summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="configureOptions">The action to perform on the bound <see cref="SwaggerOptions"/>.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" /> or <paramref name="configureOptions"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            Action<SwaggerOptions> configureOptions)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == default)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            return services.Configure(configureOptions).AddSwagger();
        }

        /// <summary>Adds <see cref="SwaggerOptions"/> and <see cref="ConfigureOptions"/> instances to <paramref name="services"/>.</summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="config">The <see cref="IConfigurationSection"/> of the <see cref="SwaggerOptions"/> instance.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" /> or <paramref name="config"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            IConfigurationSection config)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            return services.Configure<SwaggerOptions>(config).AddSwagger();
        }

        /// <summary>Adds <see cref="SwaggerOptions"/> and <see cref="ConfigureOptions"/> instances to <paramref name="services"/>.</summary>
        /// <param name="services">The <see cref="IServiceCollection"/> instance.</param>
        /// <param name="config">The <see cref="IConfigurationSection"/> of the <see cref="SwaggerOptions"/> instance.</param>
        /// <param name="configureBinder">The action to perform on the <see cref="BinderOptions"/> of the <see cref="ConfigurationBinder"/>.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" /> or <paramref name="configureBinder"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddSwagger(
            this IServiceCollection services,
            IConfigurationSection config,
            Action<BinderOptions> configureBinder)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (configureBinder == default)
            {
                throw new ArgumentNullException(nameof(configureBinder));
            }

            return services.Configure<SwaggerOptions>(config, configureBinder).AddSwagger();
        }

        private static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            SwaggerOptions options;
            using (var provider = services.BuildServiceProvider(true))
            {
                options = provider.GetRequiredService<IOptions<SwaggerOptions>>().Value;
            }

            services.AddSingleton<IConfigureOptions<SwaggerGenOptions>, ConfigureOptions>();
            services.AddSingleton<IConfigureOptions<SwaggerUIOptions>, ConfigureOptions>();
            services.AddApiVersioning(opt =>
            {
                opt.AssumeDefaultVersionWhenUnspecified = options.AssumeDefaultVersionWhenUnspecified;
                opt.ReportApiVersions = options.ReportApiVersions;
            });
            if (options.UseOData)
            {
                services.AddOData().EnableApiVersioning();
                services.AddODataApiExplorer(opt =>
                {
                    opt.AssumeDefaultVersionWhenUnspecified = options.AssumeDefaultVersionWhenUnspecified;
                    opt.SubstituteApiVersionInUrl = options.SubstituteApiVersionInUrl;
                    opt.GroupNameFormat = options.GroupNameFormat;
                });
            }
            else
            {
                services.AddVersionedApiExplorer(opt =>
                {
                    opt.AssumeDefaultVersionWhenUnspecified = options.AssumeDefaultVersionWhenUnspecified;
                    opt.SubstituteApiVersionInUrl = options.SubstituteApiVersionInUrl;
                    opt.GroupNameFormat = options.GroupNameFormat;
                });
            }

            services.AddSwaggerGen();
            return services;
        }
    }
}
