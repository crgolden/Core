namespace Core
{
    using System.Globalization;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;

    /// <inheritdoc />
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <inheritdoc />
        public string? TransformOutbound(object? value)
        {
            return value == default ? default : Regex
                .Replace($"{value}", "([a-z])([A-Z])", "$1-$2")
                .ToLower(CultureInfo.CurrentCulture);
        }
    }
}
