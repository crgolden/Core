namespace Core
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Serilog.Events;

    /// <summary>Configuration settings for Serilog.</summary>
    [PublicAPI]
    public class SerilogOptions
    {
        /// <summary>Gets or sets a value indicating whether to use scopes.</summary>
        /// <value>
        ///   <c>true</c> if set to use scopes; otherwise, <c>false</c>.</value>
        public bool Dispose { get; set; }

        /// <summary>
        /// <para>Gets the level switches.</para>
        /// <para>Level switch must be declared with a '$' sign, like "LevelSwitches" : {"$switchName" : "InitialLevel"}.</para>
        /// </summary>
        /// <value>The level switches.</value>
        public IDictionary<string, LogEventLevel> LevelSwitches { get; } = new Dictionary<string, LogEventLevel>();

        /// <summary>Gets or sets the minimum level.</summary>
        /// <value>The minimum level.</value>
        public MinimumLevel MinimumLevel { get; set; } = new MinimumLevel();

        /// <summary>Gets the enrichers.</summary>
        /// <value>The enrichers.</value>
        public IList<NamedArguments> Enrich { get; } = new List<NamedArguments>();

        /// <summary>Gets the assembly names to use.</summary>
        /// <value>The assembly names to use.</value>
        public IList<string> Using { get; } = new List<string>();

        /// <summary>Gets the properties.</summary>
        /// <value>The properties.</value>
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        /// <summary>Gets the filters.</summary>
        /// <value>The filters.</value>
        public IList<NamedArguments> Filter { get; } = new List<NamedArguments>();

        /// <summary>Gets the destructurers.</summary>
        /// <value>The destructurers.</value>
        public IList<NamedArguments> Destructure { get; } = new List<NamedArguments>();

        /// <summary>Gets the writers.</summary>
        /// <value>The writers.</value>
        public IList<NamedArguments> WriteTo { get; } = new List<NamedArguments>();

        /// <summary>Gets the auditers.</summary>
        /// <value>The auditers.</value>
        public IList<NamedArguments> AuditTo { get; } = new List<NamedArguments>();

        /// <summary>Gets or sets the Elasticsearch options.</summary>
        /// <value>The Elasticsearch options.</value>
        public ElasticsearchOptions ElasticsearchOptions { get; set; }
    }
}
