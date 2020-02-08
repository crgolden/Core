namespace Core
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;
    using Swashbuckle.AspNetCore.SwaggerUI;
    using static Microsoft.OpenApi.Models.ParameterLocation;
    using static Microsoft.OpenApi.Models.SecuritySchemeType;

    /// <summary>Configuration settings for the <see cref="SwaggerGenOptions"/> and <see cref="SwaggerUIOptions"/> classes.</summary>
    [PublicAPI]
    public class SwaggerOptions
    {
        /// <summary>Gets or sets the route prefix.</summary>
        /// <value>The route prefix.</value>
        public string RoutePrefix { get; set; }

        /// <summary>Gets or sets the XML comments file path.</summary>
        /// <value>The XML comments file path.</value>
        public string XmlCommentsFilePath { get; set; }

        /// <summary>Gets or sets the <see cref="OpenApiInfo"/>.</summary>
        /// <value>The <see cref="OpenApiInfo"/>.</value>
        [SuppressMessage("Minor Code Smell", "S1075:URIs should not be hardcoded", Justification = "Default MIT license URL provided")]
        public OpenApiInfo Info { get; set; } = new OpenApiInfo
        {
            Contact = new OpenApiContact(),
            License = new OpenApiLicense
            {
                Url = new Uri("https://opensource.org/licenses/MIT")
            }
        };

        /// <summary>Gets or sets the <see cref="OpenApiSecurityScheme"/>.</summary>
        /// <value>The <see cref="OpenApiSecurityScheme"/>.</value>
        public OpenApiSecurityScheme SecurityScheme { get; set; } = new OpenApiSecurityScheme
        {
            Name = "Authorization",
            In = Header,
            Type = OAuth2,
            Scheme = "Bearer"
        };

        /// <summary>Gets or sets the OAuth configuration.</summary>
        /// <value>The OAuth configuration.</value>
        public OAuthConfigObject OAuthConfig { get; set; }

        /// <summary>Gets or sets a value indicating whether to use OData.</summary>
        /// <value>The use OData flag.</value>
        public bool UseOData { get; set; }

        /// <summary>Gets or sets a value indicating whether to assume default version when unspecified.</summary>
        /// <value>The assume default version when unspecified flag.</value>
        // https://github.com/microsoft/aspnet-api-versioning/wiki/API-Versioning-Options#assume-default-version-when-unspecified
        public bool AssumeDefaultVersionWhenUnspecified { get; set; }

        /// <summary>Gets or sets a value indicating whether to report API versions.</summary>
        /// <value>The report API versions flag.</value>
        // https://github.com/microsoft/aspnet-api-versioning/wiki/API-Versioning-Options#report-api-versions
        public bool ReportApiVersions { get; set; } = true;

        /// <summary>Gets or sets the group name format.</summary>
        /// <value>The group name format.</value>
        // https://github.com/microsoft/aspnet-api-versioning/wiki/Version-Format
        public string GroupNameFormat { get; set; } = "'v'VVV";

        /// <summary>Gets or sets a value indicating whether to substitute the API version in the URL.</summary>
        /// <value>The substitute the API version in the URL flag.</value>
        // https://github.com/microsoft/aspnet-api-versioning/wiki/Versioning-via-the-URL-Path
        public bool SubstituteApiVersionInUrl { get; set; }
    }
}
