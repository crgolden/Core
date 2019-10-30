namespace Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class SwaggerOptions
    {
        public string? Title { get; set; }

        public string? Description { get; set; }

        public string TermsOfService { get; set; } = "Shareware";

        public string? ContactName { get; set; }

        public string? ContactEmail { get; set; }

        public Uri? ContactUrl { get; set; }

        public string LicenseName { get; set; } = "MIT";

        [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Default MIT license URL provided")]
        public Uri LicenseUrl { get; set; } = new Uri("https://opensource.org/licenses/MIT");

        public string DefaultScheme { get; set; } = "Bearer";

        public string ApiKeySchemeType { get; set; } = "apiKey";

        public string ApiKeySchemeIn { get; set; } = "Header";

        public string ApiKeySchemeName { get; set; } = "Authorization";

        public string? ApiKeySchemeDescription { get; set; }

        // https://github.com/microsoft/aspnet-api-versioning/wiki/API-Versioning-Options#assume-default-version-when-unspecified
        public bool AssumeDefaultVersionWhenUnspecified { get; set; }

        // https://github.com/microsoft/aspnet-api-versioning/wiki/API-Versioning-Options#report-api-versions
        public bool ReportApiVersions { get; set; } = true;

        // https://github.com/microsoft/aspnet-api-versioning/wiki/Version-Format
        public string GroupNameFormat { get; set; } = "'v'VVV";
    }
}
