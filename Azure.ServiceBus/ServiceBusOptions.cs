namespace Core
{
    using System.Text.Json.Serialization;
    using JetBrains.Annotations;
    using Microsoft.Azure.ServiceBus;
    using static System.String;
    using static Microsoft.Azure.ServiceBus.TransportType;

    /// <summary>Helper class for building an <see cref="IQueueClient"/> connection string.</summary>
    [PublicAPI]
    public class ServiceBusOptions
    {
        /// <summary>Gets or sets the name of the shared access key.</summary>
        /// <value>The name of the shared access key.</value>
        public string SharedAccessKeyName { get; set; }

        /// <summary>Gets or sets the primary key.</summary>
        /// <value>The primary key.</value>
        public string PrimaryKey { get; set; }

        /// <summary>Gets or sets the secondary key.</summary>
        /// <value>The secondary key.</value>
        public string SecondaryKey { get; set; }

        /// <summary>Gets or sets the endpoint.</summary>
        /// <value>The endpoint.</value>
        public string Endpoint { get; set; }

        /// <summary>Gets or sets the entity path.</summary>
        /// <value>The entity path.</value>
        public string EntityPath { get; set; }

        /// <summary>Gets or sets the type of the transport.</summary>
        /// <value>The type of the transport.</value>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TransportType? TransportType { get; set; } = Amqp;

        /// <summary>Gets the connection string.</summary>
        /// <value>The connection string.</value>
        public ServiceBusConnectionStringBuilder ConnectionStringBuilder
        {
            get
            {
                var connectionStringBuilder = new ServiceBusConnectionStringBuilder();
                if (!IsNullOrWhiteSpace(SharedAccessKeyName))
                {
                    connectionStringBuilder.SasKeyName = SharedAccessKeyName;
                }

                if (!IsNullOrWhiteSpace(PrimaryKey))
                {
                    connectionStringBuilder.SasKey = PrimaryKey;
                }
                else if (!IsNullOrWhiteSpace(SecondaryKey))
                {
                    connectionStringBuilder.SasKey = SecondaryKey;
                }

                if (!IsNullOrWhiteSpace(Endpoint))
                {
                    connectionStringBuilder.Endpoint = Endpoint;
                }

                if (!IsNullOrWhiteSpace(EntityPath))
                {
                    connectionStringBuilder.EntityPath = EntityPath;
                }

                if (TransportType.HasValue)
                {
                    connectionStringBuilder.TransportType = TransportType.Value;
                }

                return connectionStringBuilder;
            }
        }
    }
}
