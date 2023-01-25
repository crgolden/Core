namespace Core.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Common.Abilities;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Requests;
    using static System.Threading.Tasks.Task;
    using static Common.EventIds;
    using static Microsoft.Extensions.Logging.LoggerMessage;
    using static Microsoft.Extensions.Logging.LogLevel;

    /// <summary>A notification sent when a <see cref="GetRequest{T}"/> is started.</summary>
    [PublicAPI]
    public class GetNotification : INotification, ILoggable
    {
        private static readonly Action<ILogger, IReadOnlyCollection<object>, Exception> LogAction = Define<IReadOnlyCollection<object>>(Information, ReadStart, "{@KeyValues}");

        private readonly IReadOnlyCollection<object> _keyValues;
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="GetNotification"/> class.</summary>
        /// <param name="keyValues">The key values.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyValues"/> is <see langword="null" />
        /// or
        /// <paramref name="logger"/> is <see langword="null" />.</exception>
        public GetNotification(IReadOnlyCollection<object> keyValues, ILogger logger)
        {
            _keyValues = keyValues ?? throw new ArgumentNullException(nameof(keyValues));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            LogAction(_logger, _keyValues, default);
            return CompletedTask;
        }
    }
}
