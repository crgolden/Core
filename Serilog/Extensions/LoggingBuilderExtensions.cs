namespace Microsoft.Extensions.Logging
{
    using System;
    using ApplicationInsights.Extensibility;
    using Core;
    using DependencyInjection;
    using Extensions.Configuration;
    using JetBrains.Annotations;
    using Options;
    using Serilog;
    using Serilog.Events;
    using static System.Enum;
    using static System.Reflection.Assembly;
    using static System.String;
    using static System.StringComparison;
    using static ApplicationInsights.Extensibility.TelemetryConfiguration;
    using static LogLevel;
    using static Serilog.Events.LogEventLevel;
    using static Serilog.TelemetryConverter;

    /// <summary>A class with methods that extend <see cref="ILoggingBuilder"/>.</summary>
    [PublicAPI]
    public static class LoggingBuilderExtensions
    {
        /// <summary>Adds Serilog using the provided <see cref="IConfiguration"/> and <see cref="IConfigurationSection"/> to build a <see cref="SerilogOptions"/> instance.</summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="config">The config.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />
        /// or
        /// <paramref name="configuration"/> is <see langword="null" />
        /// or
        /// <paramref name="config"/> is <see langword="null" />.</exception>
        public static ILoggingBuilder AddSerilog(
            this ILoggingBuilder builder,
            IConfiguration configuration,
            IConfigurationSection config)
        {
            if (builder == default)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            builder.Services.Configure<SerilogOptions>(config);
            return AddSerilog(builder, configuration, config.Get<SerilogOptions>());
        }

        /// <summary>Adds Serilog using the provided <see cref="IConfiguration"/> and <see cref="Action{T}"/> to build a <see cref="SerilogOptions"/> instance.</summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="configureOptions">The configure options delegate.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />
        /// or
        /// <paramref name="configuration"/> is <see langword="null" />
        /// or
        /// <paramref name="configureOptions"/> is <see langword="null" />.</exception>
        public static ILoggingBuilder AddSerilog(
            this ILoggingBuilder builder,
            IConfiguration configuration,
            Action<SerilogOptions> configureOptions)
        {
            if (builder == default)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (configureOptions == default)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            builder.Services.Configure(configureOptions);
            using (var provider = builder.Services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<SerilogOptions>>();
                return AddSerilog(builder, configuration, options.Value);
            }
        }

        /// <summary>Adds Serilog using the provided <see cref="IConfiguration"/>, <see cref="IConfigurationSection"/>, and <see cref="Action{T}"/> to build a <see cref="SerilogOptions"/> instance.</summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="config">The config.</param>
        /// <param name="configureBinder">The configure binder delegate.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />
        /// or
        /// <paramref name="configuration"/> is <see langword="null" />
        /// or
        /// <paramref name="config"/> is <see langword="null" />
        /// or
        /// <paramref name="configureBinder"/> is <see langword="null" />.</exception>
        public static ILoggingBuilder AddSerilog(
            this ILoggingBuilder builder,
            IConfiguration configuration,
            IConfigurationSection config,
            Action<BinderOptions> configureBinder)
        {
            if (builder == default)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (configureBinder == default)
            {
                throw new ArgumentNullException(nameof(configureBinder));
            }

            builder.Services.Configure<SerilogOptions>(config, configureBinder);
            using (var provider = builder.Services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<SerilogOptions>>();
                return AddSerilog(builder, configuration, options.Value);
            }
        }

        /// <summary>
        /// <para>Adds Serilog using the provided <see cref="IConfiguration"/>.</para>
        /// <para>
        /// This method will build a <see cref="LoggerFilterOptions"/> instance using the default "Logging" section.
        /// To use a <see cref="SerilogOptions"/> section instead, use one of the other overloads.
        /// </para>
        /// </summary>
        /// <param name="builder">The builder.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="azureBlobConnectionString">The Azure BLOB connection string.</param>
        /// <returns>The <paramref name="builder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="builder"/> is <see langword="null" />
        /// or
        /// <paramref name="configuration"/> is <see langword="null" />.</exception>
        public static ILoggingBuilder AddSerilog(
            this ILoggingBuilder builder,
            IConfiguration configuration,
            string azureBlobConnectionString = default)
        {
            if (builder == default)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var config = new LoggerConfiguration();

            BuildFromRules(config, builder.Services, configuration, azureBlobConnectionString);
            var logger = config.CreateLogger();
            builder.AddSerilog(logger);
            return builder;
        }

        private static ILoggingBuilder AddSerilog(
            ILoggingBuilder builder,
            IConfiguration configuration,
            SerilogOptions options)
        {
            if (options == default)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var config = new LoggerConfiguration();
            config.ReadFrom.Configuration(configuration, nameof(SerilogOptions));
            if (options.ElasticsearchOptions != default)
            {
                config.WriteTo.Elasticsearch(options.ElasticsearchOptions.SinkOptions);
            }

            var logger = config.CreateLogger();
            builder.AddSerilog(logger, options.Dispose);
            return builder;
        }

        private static LoggerFilterOptions GetLoggerFilterOptions(IConfiguration configuration)
        {
            const string logLevelKey = "LogLevel";
            var options = new LoggerFilterOptions();
            foreach (var configurationSection in configuration.GetChildren())
            {
                if (configurationSection.Key.Equals(logLevelKey, OrdinalIgnoreCase))
                {
                    LoadRules(options, configurationSection, default);
                }
                else
                {
                    var logLevelSection = configurationSection.GetSection(logLevelKey);
                    if (logLevelSection == default)
                    {
                        continue;
                    }

                    LoadRules(options, logLevelSection, configurationSection.Key);
                }
            }

            return options;
        }

        private static void LoadRules(LoggerFilterOptions options, IConfiguration configuration, string logger)
        {
            const string defaultCategory = "Default";
            foreach (var section in configuration.AsEnumerable(true))
            {
                if (!TryGetSwitch(section.Value, out var level))
                {
                    continue;
                }

                options.Rules.Add(section.Key.Equals(defaultCategory, OrdinalIgnoreCase)
                    ? new LoggerFilterRule(logger, null, level, null)
                    : new LoggerFilterRule(logger, section.Key, level, null));
            }
        }

        private static bool TryGetSwitch(string value, out LogLevel level)
        {
            if (IsNullOrWhiteSpace(value))
            {
                level = None;
                return false;
            }

            if (TryParse(value, true, out level))
            {
                return true;
            }

            throw new InvalidOperationException($"Configuration value '{value}' is not supported.");
        }

        private static LogEventLevel? GetLogEventLevel(LogLevel? logLevel)
        {
            switch (logLevel)
            {
                case Trace:
                    return Verbose;
                case LogLevel.Debug:
                    return LogEventLevel.Debug;
                case LogLevel.Information:
                    return LogEventLevel.Information;
                case LogLevel.Warning:
                    return LogEventLevel.Warning;
                case LogLevel.Error:
                    return LogEventLevel.Error;
                case Critical:
                    return Fatal;
                case None:
                    return default;
                default:
                    return default;
            }
        }

        private static void BuildFromRules(
            LoggerConfiguration config,
            IServiceCollection services,
            IConfiguration configuration,
            string azureBlobConnectionString)
        {
            var loggerFilterOptions = GetLoggerFilterOptions(configuration);
            var minimumLevel = GetLogEventLevel(loggerFilterOptions.MinLevel);
            config.MinimumLevel.Is(minimumLevel ?? LogEventLevel.Information);
            foreach (var rule in loggerFilterOptions.Rules)
            {
                var categoryLevel = GetLogEventLevel(rule.LogLevel);
                if (string.Equals("Console", rule.ProviderName, OrdinalIgnoreCase))
                {
                    config.WriteTo.Console(categoryLevel ?? Verbose);
                    continue;
                }

                if (string.Equals("Debug", rule.ProviderName, OrdinalIgnoreCase))
                {
                    config.WriteTo.Debug(categoryLevel ?? Verbose);
                    continue;
                }

                if (string.Equals("EventLog", rule.ProviderName, OrdinalIgnoreCase))
                {
                    var source = GetEntryAssembly()?.FullName;
                    if (IsNullOrWhiteSpace(source))
                    {
                        continue;
                    }

                    config.WriteTo.EventLog(source, restrictedToMinimumLevel: categoryLevel ?? Verbose);
                    continue;
                }

                if (string.Equals("AzureAppServicesBlob", rule.ProviderName, OrdinalIgnoreCase) && !IsNullOrWhiteSpace(azureBlobConnectionString))
                {
                    config.WriteTo.AzureBlobStorage(azureBlobConnectionString, categoryLevel ?? Verbose);
                    continue;
                }

                if (string.Equals("ApplicationInsights", rule.ProviderName, OrdinalIgnoreCase))
                {
                    TelemetryConfiguration telemetryConfiguration;
                    using (var provider = services.BuildServiceProvider(true))
                    {
                        var options = provider.GetService<IOptions<TelemetryConfiguration>>();
                        telemetryConfiguration = options?.Value;
                    }

#pragma warning disable CA2000 // Dispose objects before losing scope
                    config.WriteTo.ApplicationInsights(telemetryConfiguration ?? CreateDefault(), Events, categoryLevel ?? Verbose);
#pragma warning restore CA2000 // Dispose objects before losing scope
                    continue;
                }

                if (!IsNullOrWhiteSpace(rule.CategoryName) && categoryLevel.HasValue)
                {
                    config.MinimumLevel.Override(rule.CategoryName, categoryLevel.Value);
                }
            }
        }
    }
}
