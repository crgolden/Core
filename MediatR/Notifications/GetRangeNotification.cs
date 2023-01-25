namespace Core.Notifications
{
    using System;
    using System.Threading.Tasks;
    using Common.Abilities;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Requests;
    using static System.String;
    using static System.Threading.Tasks.Task;
    using static Common.EventIds;
    using static Microsoft.Extensions.Logging.LoggerMessage;
    using static Microsoft.Extensions.Logging.LogLevel;

    /// <summary>A notification sent when a <see cref="GetRangeRequest{T}"/> is started.</summary>
    [PublicAPI]
    public class GetRangeNotification : INotification, ILoggable
    {
        private static readonly Action<ILogger, Exception> LogAction = Define(Information, ReadRangeStart, Empty);

        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="GetRangeNotification"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        public GetRangeNotification(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            LogAction(_logger, default);
            return CompletedTask;
        }
    }
}
