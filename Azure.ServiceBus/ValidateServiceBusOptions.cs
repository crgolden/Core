namespace Core
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Options;
    using static System.String;
    using static Microsoft.Extensions.Options.ValidateOptionsResult;

    /// <inheritdoc />
    [PublicAPI]
    public class ValidateServiceBusOptions : IValidateOptions<ServiceBusOptions>
    {
        /// <inheritdoc />
        public ValidateOptionsResult Validate(string name, ServiceBusOptions options)
        {
            if (options == default)
            {
                throw new ArgumentNullException(nameof(options));
            }

            return IsNullOrWhiteSpace(options.EntityPath) ||
                   IsNullOrWhiteSpace(options.Endpoint) ||
                   IsNullOrWhiteSpace(options.SharedAccessKeyName) ||
                   (IsNullOrWhiteSpace(options.PrimaryKey) && IsNullOrWhiteSpace(options.SecondaryKey)) ||
                   options.TransportType == default
                ? Fail($"'{nameof(ServiceBusOptions)}' section is invalid")
                : Success;
        }
    }
}
