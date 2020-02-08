namespace Core
{
    using JetBrains.Annotations;

    /// <summary>Configuration settings for <see cref="IdentityServer4"/>.</summary>
    [PublicAPI]
    public class IdentityServerOptions
    {
        /// <summary>Gets or sets the default scheme.</summary>
        /// <value>The default scheme.</value>
        public string DefaultScheme { get; set; }

        /// <summary>Gets or sets the signing credential.</summary>
        /// <value>The signing credential.</value>
        public string SigningCredential { get; set; }

        /// <summary>Gets or sets the validation key.</summary>
        /// <value>The validation key.</value>
        public string ValidationKey { get; set; }

        public bool UseIdentity { get; set; } = true;

        public bool UseEntityFramework { get; set; } = true;

        public bool UseAuthentication { get; set; } = true;

        public bool UseIdentityServer4Authentication { get; set; } = true;

        public bool UseFacebookAuthentication { get; set; }
    }
}
