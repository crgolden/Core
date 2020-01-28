namespace Core
{
    using System.Collections.Generic;

    public class NamedArguments
    {
        public string Name { get; set; }

        public IDictionary<string, object> Args { get; set; } = new Dictionary<string, object>();
    }
}
