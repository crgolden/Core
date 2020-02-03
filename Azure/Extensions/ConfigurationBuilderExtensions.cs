namespace Microsoft.Extensions.Configuration
{
    using System;
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
        /// <returns>The <paramref name="configBuilder"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="configBuilder"/> is <see langword="null" />.</exception>
        public static IConfigurationBuilder AddAzureKeyVault(this IConfigurationBuilder configBuilder)
        {
            if (configBuilder == default)
            {
                throw new ArgumentNullException(nameof(configBuilder));
            }

            var configRoot = configBuilder.Build();
            var keyVaultName = configRoot.GetValue<string>("KeyVaultName");
            if (IsNullOrWhiteSpace(keyVaultName))
            {
                return configBuilder;
            }

            var azureServiceTokenProvider = new AzureServiceTokenProvider();
            var authenticationCallback = new AuthenticationCallback(azureServiceTokenProvider.KeyVaultTokenCallback);
            var vault = $"https://{keyVaultName}.vault.azure.net/";
            var manager = new DefaultKeyVaultSecretManager();
            using (var keyVaultClient = new KeyVaultClient(authenticationCallback))
            {
                configBuilder.AddAzureKeyVault(vault, keyVaultClient, manager);
            }

            return configBuilder;
        }
    }
}
