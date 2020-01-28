namespace Core.Extensions
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using Serilog;
    using Serilog.Events;
    using static System.StringComparison;
    using static Microsoft.Extensions.Logging.LogLevel;
    using static Serilog.Events.LogEventLevel;

    public static class HostBuilderExtensions
    {
        public static IHostBuilder UseElasticsearchSerilog(this IHostBuilder hostBuilder)
        {
            return hostBuilder.UseSerilog((context, loggerConfiguration) =>
            {
                var section = context.Configuration.GetSection(nameof(ElasticsearchOptions));
                if (!section.Exists())
                {
                    throw new InvalidOperationException($"Missing '{nameof(ElasticsearchOptions)}' section");
                }

                loggerConfiguration.Enrich.FromLogContext().WriteTo.Console().WriteTo.Elasticsearch(section.Get<ElasticsearchOptions>().SinkOptions);
                var options = GetLoggerFilterOptions(context.Configuration);
                var minimumLevel = GetLogEventLevel(options.MinLevel);
                loggerConfiguration.MinimumLevel.Is(minimumLevel ?? LogEventLevel.Information);
                foreach (var rule in options.Rules)
                {
                    var categoryLevel = GetLogEventLevel(rule.LogLevel);
                    if (string.Equals("Console", rule.ProviderName, OrdinalIgnoreCase))
                    {
                        loggerConfiguration.WriteTo.Console(categoryLevel ?? Verbose);
                        continue;
                    }
                    
                    if (string.Equals("Debug", rule.ProviderName, OrdinalIgnoreCase))
                    {
                        loggerConfiguration.WriteTo.Debug(categoryLevel ?? Verbose);
                        continue;
                    }
                    
                    if (string.Equals("EventLog", rule.ProviderName, OrdinalIgnoreCase))
                    {
                        var source = Assembly.GetEntryAssembly()?.FullName;
                        loggerConfiguration.WriteTo.EventLog(source, restrictedToMinimumLevel: categoryLevel ?? Verbose);
                        continue;
                    }
                    
                    if (string.Equals("", rule.ProviderName, OrdinalIgnoreCase))

                    if (!string.IsNullOrWhiteSpace(rule.CategoryName))
                    {
                        loggerConfiguration.MinimumLevel.Override(rule.CategoryName, categoryLevel.Value);
                    }
                }
            });
        }

        private static LoggerFilterOptions GetLoggerFilterOptions(IConfiguration configuration)
        {
            var options = new LoggerFilterOptions();
            foreach (var configurationSection in configuration.GetChildren())
            {
                if (configurationSection.Key.Equals("LogLevel", OrdinalIgnoreCase))
                {
                    LoadRules(options, configurationSection, null);
                }
                else
                {
                    var logLevelSection = configurationSection.GetSection("LogLevel");
                    if (logLevelSection == null)
                    {
                        continue;
                    }

                    LoadRules(options, logLevelSection, configurationSection.Key);
                }
            }

            return options;
        }

        private static void LoadRules(LoggerFilterOptions options, IConfiguration configurationSection, string logger)
        {
            foreach (var section in configurationSection.AsEnumerable(true))
            {
                if (!TryGetSwitch(section.Value, out var level))
                {
                    continue;
                }

                var category = section.Key;
                if (category.Equals("Default", OrdinalIgnoreCase))
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
    }
}
