namespace Core
{
    using JetBrains.Annotations;

    /// <summary>Configuration settings for Elasticsearch.</summary>
    [PublicAPI]
    public class ElasticsearchOptions
    {
        /// <summary>Gets or sets the admin username.</summary>
        /// <value>The admin username.</value>
        public string AdminUsername { get; set; }

        /// <summary>Gets or sets the admin password.</summary>
        /// <value>The admin password.</value>
        public string AdminPassword { get; set; }

        /// <summary>Gets or sets the kibana username.</summary>
        /// <value>The Kibana username.</value>
        public string KibanaUsername { get; set; }

        /// <summary>Gets or sets the kibana password.</summary>
        /// <value>The Kibana password.</value>
        public string KibanaPassword { get; set; }

        /// <summary>Gets or sets the logstash username.</summary>
        /// <value>The Logstash username.</value>
        public string LogstashUsername { get; set; }

        /// <summary>Gets or sets the logstash password.</summary>
        /// <value>The Logstash password.</value>
        public string LogstashPassword { get; set; }

        /// <summary>Gets or sets the beats username.</summary>
        /// <value>The Beats username.</value>
        public string BeatsUsername { get; set; }

        /// <summary>Gets or sets the beats password.</summary>
        /// <value>The Beats password.</value>
        public string BeatsPassword { get; set; }
    }
}
