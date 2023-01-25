namespace Core
{
    using System;
    using Azure.Messaging.ServiceBus;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using static System.String;
    using static Microsoft.Extensions.Options.ValidateOptionsResult;

    /// <inheritdoc />
    [PublicAPI]
    public class ValidateServiceBusConnectionStringProperties : IValidateOptions<ServiceBusConnectionStringProperties>
    {
        /// <inheritdoc />
        public ValidateOptionsResult Validate(string name, ServiceBusConnectionStringProperties options)
        {
            if (options == default)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return IsNullOrWhiteSpace(options.EntityPath) ||
                   options.Endpoint == default ||
                   IsNullOrWhiteSpace(options.SharedAccessKeyName) ||
                   IsNullOrWhiteSpace(options.SharedAccessKey) ||
                   IsNullOrWhiteSpace(options.FullyQualifiedNamespace)
                ? Fail($"'{nameof(ValidateServiceBusConnectionStringProperties)}' section is invalid")
                : Success;
        }
    }
}
