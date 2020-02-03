namespace Core.Notifications
{
    using System;
    using System.Linq.Expressions;
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

    /// <summary>A notification sent when a <see cref="DeleteRangeRequest{T}"/> is completed.</summary>
    [PublicAPI]
    public class DeleteRangeNotification : INotification, ILoggable
    {
        internal static readonly Action<ILogger, Expression, Exception> StartAction = Define<Expression>(Information, DeleteRangeStart, "{@Expression}");
        private static readonly Action<ILogger, Unit, Exception> FinishAction = Define<Unit>(Information, DeleteRangeEnd, "{@Result}");

        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="DeleteRangeNotification"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        public DeleteRangeNotification(ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            FinishAction(_logger, Value, default);
            return CompletedTask;
        }
    }
}
