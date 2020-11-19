namespace Microsoft.Extensions.Configuration
{
    using System;
    using global::Azure.Identity;
    using JetBrains.Annotations;
    using static System.String;

    /// <summary>A class with methods that extend <see cref="IConfigurationBuilder"/>.</summary>
    [PublicAPI]
    public static class ConfigurationBuilderExtensions
    {
        /// <summary>Adds the Azure Key Vault.</summary>
        /// <param name="configBuilder">The configuration builder.</param>
        /// <param name="keyVaultName">The key vault name.</param>
        /// <returns>The <paramref name="configBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configBuilder"/> is <see langword="null" />
        /// or
        /// <paramref name="keyVaultName"/> is <see langword="null" />.</exception>
        public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder configBuilder, string keyVaultName)
        {
            if (configBuilder == default)
            {
                throw new ArgumentNullException(nameof(configBuilder));
            }

            if (IsNullOrWhiteSpace(keyVaultName))
            {
                throw new ArgumentNullException(nameof(keyVaultName));
            }

            var vaultUri = new Uri($"https://{keyVaultName}.vault.azure.net/");
            var credential = new DefaultAzureCredential();
            return configBuilder.AddAzureKeyVault(vaultUri, credential);
        }
    }
}
