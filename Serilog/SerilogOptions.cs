namespace Core
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Serilog.Events;

    [PublicAPI]
    public class SerilogOptions
    {
        public bool Dispose { get; set; }

        /// <summary>
        /// <para>Gets or sets the level switches.</para>
        /// <para>Level switch must be declared with a '$' sign, like "LevelSwitches" : {"$switchName" : "InitialLevel"}.</para>
        /// </summary>
        /// <value>The level switches.</value>
        public IDictionary<string, LogEventLevel> LevelSwitches { get; set; } = new Dictionary<string, LogEventLevel>();

        public MinimumLevel MinimumLevel { get; set; }

        public IList<NamedArguments> Enrich { get; set; } = new List<NamedArguments>();

        /// <summary>Gets or sets the assembly names to use.</summary>
        /// <value>The assembly names to use.</value>
        public IList<string> Using { get; set; } = new List<string>();

        public IDictionary<string, object> Properties { get; set; } = new Dictionary<string, object>();

        public IList<NamedArguments> Filter { get; set; } = new List<NamedArguments>();

        public IList<NamedArguments> Destructure { get; set; } = new List<NamedArguments>();

        public IList<NamedArguments> WriteTo { get; set; } = new List<NamedArguments>();

        public IList<NamedArguments> AuditTo { get; set; } = new List<NamedArguments>();

        public ElasticsearchOptions ElasticsearchOptions { get; set; }
    }
}
