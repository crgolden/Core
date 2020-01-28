namespace Core.Extensions
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Serilog;
    using Serilog.Events;
    using static System.Reflection.Assembly;
    using static System.StringComparison;
    using static Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration;
    using static Microsoft.Extensions.Logging.LogLevel;
    using static Serilog.Events.LogEventLevel;
    using static Serilog.TelemetryConverter;

    [PublicAPI]
    public static class LoggingBuilderExtensions
    {
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
            BuildFromRules(config, configuration, azureBlobConnectionString);
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

                var category = section.Key;
                if (category.Equals(defaultCategory, OrdinalIgnoreCase))
                {
                    category = null;
                }

                var newRule = new LoggerFilterRule(logger, category, level, null);
                options.Rules.Add(newRule);
            }
        }

        private static bool TryGetSwitch(string value, out LogLevel level)
        {
            if (string.IsNullOrEmpty(value))
            {
                level = None;
                return false;
            }

            if (Enum.TryParse(value, true, out level))
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
                    config.WriteTo.EventLog(source, restrictedToMinimumLevel: categoryLevel ?? Verbose);
                    continue;
                }

                if (string.Equals("AzureAppServicesBlob", rule.ProviderName, OrdinalIgnoreCase) &&
                    !string.IsNullOrWhiteSpace(azureBlobConnectionString))
                {
                    config.WriteTo.AzureBlobStorage(azureBlobConnectionString, categoryLevel ?? Verbose);
                    continue;
                }

                if (string.Equals("ApplicationInsights", rule.ProviderName, OrdinalIgnoreCase))
                {
                    config.WriteTo.ApplicationInsights(Active, Events, categoryLevel ?? Verbose);
                    continue;
                }

                if (!string.IsNullOrWhiteSpace(rule.CategoryName) && categoryLevel.HasValue)
                {
                    config.MinimumLevel.Override(rule.CategoryName, categoryLevel.Value);
                }
            }
        }
    }
}
