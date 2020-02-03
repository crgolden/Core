namespace Core.Notifications
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Requests;
    using static System.Threading.Tasks.Task;
    using static Common.EventIds;
    using static MediatR.Unit;
    using static Microsoft.Extensions.Logging.LoggerMessage;
    using static Microsoft.Extensions.Logging.LogLevel;

    /// <summary>A notification sent when a <see cref="CreateRequest{T}"/> is completed.</summary>
    [PublicAPI]
    public class CreateNotification : INotification, ILoggable
    {
        private static readonly Action<ILogger, Unit, Exception> LogAction = Define<Unit>(Information, CreateEnd, "{@Result}");

        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="CreateNotification"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        public CreateNotification(ILogger logger)
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
