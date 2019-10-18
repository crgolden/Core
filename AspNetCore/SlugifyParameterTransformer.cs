namespace Core
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string? TransformOutbound(object value)
        {
            return value == default ? default : Regex
                .Replace($"{value}", "([a-z])([A-Z])", "$1-$2")
                .ToLower(CultureInfo.CurrentCulture);
        }
    }
}
