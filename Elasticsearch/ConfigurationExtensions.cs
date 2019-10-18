namespace Core
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Extensions.Configuration;

    public static class ConfigurationExtensions
    {
        public static IEnumerable<Uri> GetLogNodes(this IConfiguration configuration)
        {
            if (configuration == default)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var logNodes = Array.Empty<Uri>();
            var section = configuration.GetSection(nameof(ElasticsearchOptions));
            if (!section.Exists())
            {
                return logNodes;
            }

            var options = section.Get<ElasticsearchOptions>();
            return options.LogNodes == null
                ? logNodes
                : options.LogNodes.Select(logNode => new Uri(logNode));
        }
    }
}
