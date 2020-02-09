namespace Core.ParameterTransformers
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Routing;
    using static System.Globalization.CultureInfo;
    using static System.Text.RegularExpressions.Regex;

    /// <inheritdoc />
    [PublicAPI]
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        /// <inheritdoc />
        public string TransformOutbound(object value)
        {
            if (value == default)
            {
                throw new ArgumentNullException(nameof(value));
            }

            return Replace($"{value}", "([a-z])([A-Z])", "$1-$2").ToLower(CurrentCulture);
        }
    }
}
