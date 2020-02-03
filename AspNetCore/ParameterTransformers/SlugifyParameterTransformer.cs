namespace Core.ParameterTransformers
{
    using System;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;
    using static System.Globalization.CultureInfo;

    /// <inheritdoc />
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <inheritdoc />
        public string TransformOutbound(object value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Regex.Replace($"{value}", "([a-z])([A-Z])", "$1-$2").ToLower(CurrentCulture);
        }
    }
}
