namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Text;
    using Configuration;
    using Core;
    using global::Azure.Messaging.ServiceBus;
    using JetBrains.Annotations;
    using Options;
    using static System.String;

    /// <summary>A class with methods that extend <see cref="IServiceCollection"/>.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds a <see cref="ServiceBusClient"/> using the provided <paramref name="configureOptions"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configureOptions">The config.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="configureOptions"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddServiceBusClient(
            this IServiceCollection services,
            Action<ServiceBusConnectionStringProperties> configureOptions)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == default)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddSingleton<IValidateOptions<ServiceBusConnectionStringProperties>, ValidateServiceBusConnectionStringProperties>();
            services.Configure(configureOptions);
            using var provider = services.BuildServiceProvider(true);
            var options = provider.GetRequiredService<IOptions<ServiceBusConnectionStringProperties>>().Value;
            return AddServiceBusClient(services, options);
        }

        /// <summary>Adds a <see cref="ServiceBusClient"/> using the provided <paramref name="config"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="config"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddServiceBusClient(
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

            services.AddSingleton<IValidateOptions<ServiceBusConnectionStringProperties>, ValidateServiceBusConnectionStringProperties>();
            services.Configure<ServiceBusConnectionStringProperties>(config);
            using var provider = services.BuildServiceProvider(true);
            var options = provider.GetRequiredService<IOptions<ServiceBusConnectionStringProperties>>().Value;
            return services.AddServiceBusClient(options);
        }

        /// <summary>Adds a scoped <see cref="ServiceBusClient"/> using the provided <paramref name="config"/> and <paramref name="configureBinder"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <param name="configureBinder">The configure binder.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="config"/> is <see langword="null" />
        /// or
        /// <paramref name="configureBinder"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddServiceBusClient(
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

            services.AddSingleton<IValidateOptions<ServiceBusConnectionStringProperties>, ValidateServiceBusConnectionStringProperties>();
            services.Configure<ServiceBusConnectionStringProperties>(config, configureBinder);
            using var provider = services.BuildServiceProvider(true);
            var options = provider.GetRequiredService<IOptions<ServiceBusConnectionStringProperties>>().Value;
            return services.AddServiceBusClient(options);
        }

        private static IServiceCollection AddServiceBusClient(this IServiceCollection services, ServiceBusConnectionStringProperties options)
        {
            const char keyValueSeparator = '=', keyValuePairDelimiter = ';';
            var connectionStringBuilder = new StringBuilder();
            if (options.Endpoint != default)
            {
                connectionStringBuilder.Append(nameof(options.Endpoint)).Append(keyValueSeparator).Append(options.Endpoint).Append(keyValuePairDelimiter);
            }

            if (!IsNullOrWhiteSpace(options.SharedAccessKeyName))
            {
                connectionStringBuilder.Append(nameof(options.SharedAccessKeyName)).Append(keyValueSeparator).Append(options.SharedAccessKeyName).Append(keyValuePairDelimiter);
            }

            if (!IsNullOrWhiteSpace(options.SharedAccessKey))
            {
                connectionStringBuilder.Append(nameof(options.SharedAccessKey)).Append(keyValueSeparator).Append(options.SharedAccessKey).Append(keyValuePairDelimiter);
            }

            if (!IsNullOrWhiteSpace(options.SharedAccessSignature))
            {
                connectionStringBuilder.Append(nameof(options.SharedAccessSignature)).Append(keyValueSeparator).Append(options.SharedAccessSignature).Append(keyValuePairDelimiter);
            }

            if (!IsNullOrWhiteSpace(options.EntityPath))
            {
                connectionStringBuilder.Append(nameof(options.EntityPath)).Append(keyValueSeparator).Append(options.EntityPath);
            }

            var connectionString = connectionStringBuilder.ToString();
#pragma warning disable CA2000 // Dispose objects before losing scope
            var serviceBusClient = new ServiceBusClient(connectionString);
#pragma warning restore CA2000 // Dispose objects before losing scope
            return services.AddSingleton(serviceBusClient);
        }
    }
}
