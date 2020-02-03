namespace Microsoft.AspNetCore.Http
{
    using System;
    using JetBrains.Annotations;
    using static System.String;
    using static Microsoft.Net.Http.Headers.HeaderNames;

    /// <summary>A class with methods that extend <see cref="HttpRequest"/>.</summary>
    [PublicAPI]
    public static class HttpRequestExtensions
    {
        /// <summary>Gets the origin.</summary>
        /// <param name="request">The request.</param>
        /// <returns>The origin.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="request"/> is <see langword="null"/>.</exception>
        public static Uri GetOrigin(this HttpRequest request)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return request.Headers.TryGetValue(Referer, out var referer)
                ? new Uri(referer)
                : new Uri(Concat(
                    request.Scheme,
                    "://",
                    request.Host.ToUriComponent(),
                    request.PathBase.ToUriComponent(),
                    request.Path.ToUriComponent(),
                    request.QueryString.ToUriComponent()));
        }
    }
}
