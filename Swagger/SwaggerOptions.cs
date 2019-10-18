﻿namespace Core
{
    using System;

    public class SwaggerOptions
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string TermsOfService { get; set; } = "Shareware";

        public string? ContactName { get; set; }

        public string? ContactEmail { get; set; }

        public Uri? ContactUrl { get; set; }

        public string LicenseName { get; set; } = "MIT";

        public Uri LicenseUrl { get; set; } = new Uri("https://opensource.org/licenses/MIT");

        public string DefaultScheme { get; set; } = "Bearer";

        public string ApiKeySchemeType { get; set; } = "apiKey";

        public string ApiKeySchemeIn { get; set; } = "Header";

        public string ApiKeySchemeName { get; set; } = "Authorization";

        public string? ApiKeySchemeDescription { get; set; }
    }
}