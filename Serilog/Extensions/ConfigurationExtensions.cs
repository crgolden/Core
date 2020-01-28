namespace Core.Extensions
{
    using System;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static IConfigurationSection GetSerilogOptionsSection(this IConfiguration configuration)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var section = configuration.GetSection(nameof(SerilogOptions));
            if (!section.Exists())
            {
                throw new ArgumentException($"'{nameof(SerilogOptions)}' section doesn't exist", nameof(configuration));
            }

            return section;
        }
    }
}