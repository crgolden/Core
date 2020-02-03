namespace Core
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Serilog.Events;

    /// <summary>The minimum log level.</summary>
    [PublicAPI]
    public class MinimumLevel
    {
        /// <summary>Gets or sets the default.</summary>
        /// <value>The default.</value>
        public LogEventLevel Default { get; set; }

        /// <summary>Gets or sets the controlled by.</summary>
        /// <value>The controlled by.</value>
        public string ControlledBy { get; set; }

        /// <summary>Gets the override.</summary>
        /// <value>The override.</value>
        public IDictionary<string, LogEventLevel> Override { get; } = new Dictionary<string, LogEventLevel>();
    }
}
