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

        /// <summary>Gets or sets a value indicating whether to use Identity.</summary>
        /// <value>
        /// <see langword="true"/> if set to use Identity; otherwise, <see langword="false"/>.
        /// </value>
        public bool UseIdentity { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use Entity Framework.</summary>
        /// <value>
        /// <see langword="true"/> if set to use Entity Framework; otherwise, <see langword="false"/>.
        /// </value>
        public bool UseEntityFramework { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use authentication.</summary>
        /// <value>
        /// <see langword="true"/> if set to use authentication; otherwise, <see langword="false"/>.
        /// </value>
        public bool UseAuthentication { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use Identity Server 4 Authentication.</summary>
        /// <value>
        /// <see langword="true"/> if set to use Identity Server 4 Authentication; otherwise, <see langword="false"/>.
        /// </value>
        public bool UseIdentityServer4Authentication { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use Facebook authentication.</summary>
        /// <value>
        /// <see langword="true"/> if set to use Facebook Authentication; otherwise, <see langword="false"/>.
        /// </value>
        public bool UseFacebookAuthentication { get; set; }
    }
}
