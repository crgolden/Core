namespace Core
{
    using System.Collections.Generic;
    using Serilog.Events;

    public class MinimumLevel
    {
        public LogEventLevel Default { get; set; }

        public string ControlledBy { get; set; }

        public IDictionary<string, LogEventLevel> Override { get; set; } = new Dictionary<string, LogEventLevel>();
    }
}
