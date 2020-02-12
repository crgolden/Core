namespace Microsoft.Extensions.Configuration
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Azure.KeyVault;
    using Azure.Services.AppAuthentication;
    using AzureKeyVault;
    using JetBrains.Annotations;
    using static System.String;
    using static Azure.KeyVault.KeyVaultClient;

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
        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose client")]
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

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var authenticationCallback = new AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);
            var vault = $"https://{keyVaultName}.vault.azure.net/";
            var manager = new DefaultKeyVaultSecretManager();
            var client = new KeyVaultClient(authenticationCallback);
            return configBuilder.AddAzureKeyVault(vault, client, manager);
        }
    }
}
