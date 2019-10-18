namespace Core
{
    using System.Collections.Generic;

    public class ElasticsearchOptions
    {
        public string? AdminUsername { get; set; }

        public string? AdminPassword { get; set; }

        public string? KibanaUsername { get; set; }

        public string? KibanaPassword { get; set; }

        public string? LogstashUsername { get; set; }

        public string? LogstashPassword { get; set; }

        public string? BeatsUsername { get; set; }

        public string? BeatsPassword { get; set; }

        public IList<string>? DataNodes { get; set; }

        public IList<string>? LogNodes { get; set; }
    }
}