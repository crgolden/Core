namespace Core
{
    using System;
    using Microsoft.AspNetCore.Http;
    using static Microsoft.Net.Http.Headers.HeaderNames;

    public static class HttpRequestExtensions
    {
        public static string GetOrigin(this HttpRequest request)
        {
            string origin;

            if (request.Headers.TryGetValue(Referer, out var referer))
            {
                var uri = new Uri(referer);
                origin = $"{uri.GetLeftPart(UriPartial.Scheme)}{uri.Host}";
            }
            else
            {
                origin = $"{request.Scheme}://{request.Host}";
            }

            return origin;
        }
    }
}
