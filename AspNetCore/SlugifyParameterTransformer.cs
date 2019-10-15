﻿namespace Core
{
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Routing;

    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value)
        {
            return value == null
                ? null
                : Regex.Replace($"{value}", "([a-z])([A-Z])", "$1-$2").ToLowerInvariant();
        }
    }
}
