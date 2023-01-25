namespace Core.Notifications
{
    using System;
    using System.Linq.Expressions;
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

    /// <summary>A notification sent when a <see cref="DeleteRequest{T}"/> is completed.</summary>
    [PublicAPI]
    public class DeleteNotification : INotification, ILoggable
    {
        internal static readonly Action<ILogger, Expression, Exception> StartAction = Define<Expression>(Information, DeleteStart, "{@Expression}");
        private static readonly Action<ILogger, Unit, Exception> FinishAction = Define<Unit>(Information, DeleteEnd, "{@Result}");

        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="DeleteNotification"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        public DeleteNotification(ILogger logger)
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