namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using Azure.ServiceBus;
    using Azure.ServiceBus.Core;
    using Configuration;
    using Core;
    using JetBrains.Annotations;
    using Options;

    /// <summary>A class with methods that extend <see cref="IServiceCollection"/>.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds a <see cref="QueueClient"/> using the provided <paramref name="configureOptions"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configureOptions">The config.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="configureOptions"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddQueueClient(
            this IServiceCollection services,
            Action<ServiceBusOptions> configureOptions)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == default)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.AddSingleton<IValidateOptions<ServiceBusOptions>, ValidateServiceBusOptions>();
            services.Configure(configureOptions);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
                return services.AddQueueClient(options);
            }
        }

        /// <summary>Adds a <see cref="QueueClient"/> using the provided <paramref name="config"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="config"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddQueueClient(
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

            services.AddSingleton<IValidateOptions<ServiceBusOptions>, ValidateServiceBusOptions>();
            services.Configure<ServiceBusOptions>(config);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
                return services.AddQueueClient(options);
            }
        }

        /// <summary>Adds a scoped <see cref="QueueClient"/> using the provided <paramref name="config"/> and <paramref name="configureBinder"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <param name="configureBinder">The configure binder.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="config"/> is <see langword="null" />
        /// or
        /// <paramref name="configureBinder"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddQueueClient(
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

            services.AddSingleton<IValidateOptions<ServiceBusOptions>, ValidateServiceBusOptions>();
            services.Configure<ServiceBusOptions>(config, configureBinder);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<ServiceBusOptions>>().Value;
                return services.AddQueueClient(options);
            }
        }

        private static IServiceCollection AddQueueClient(this IServiceCollection services, ServiceBusOptions options)
        {
            services.AddSingleton(new QueueClient(options.ConnectionStringBuilder));
            services.AddSingleton<IClientEntity>(sp => sp.GetRequiredService<QueueClient>());
            services.AddSingleton<IQueueClient>(sp => sp.GetRequiredService<QueueClient>());
            services.AddSingleton<IReceiverClient>(sp => sp.GetRequiredService<QueueClient>());
            services.AddSingleton<ISenderClient>(sp => sp.GetRequiredService<QueueClient>());
            return services;
        }
    }
}
