namespace Core.Notifications
{
    using System;
    using System.Threading.Tasks;
    using Common.Abilities;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Requests;
    using static System.Threading.Tasks.Task;
    using static Common.EventIds;
    using static MediatR.Unit;
    using static Microsoft.Extensions.Logging.LoggerMessage;
    using static Microsoft.Extensions.Logging.LogLevel;

    /// <summary>A notification sent when a <see cref="UpdateRangeRequest{T}"/> is completed.</summary>
    [PublicAPI]
    public class UpdateRangeNotification : INotification, ILoggable
    {
        private static readonly Action<ILogger, Unit, Exception> LogAction = Define<Unit>(Information, UpdateRangeEnd, "{@Result}");

        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="UpdateRangeNotification"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        public UpdateRangeNotification(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            LogAction(_logger, Value, default);
            return CompletedTask;
        }
    }
}
