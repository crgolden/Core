namespace Core
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>The named arguments.</summary>
    [PublicAPI]
    public class NamedArguments
    {
        /// <summary>Gets or sets the name.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>Gets the arguments.</summary>
        /// <value>The arguments.</value>
        public IDictionary<string, object> Args { get; } = new Dictionary<string, object>();
    }
}
