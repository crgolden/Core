namespace Core.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;
    using Common;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using Requests;
    using static System.Threading.Tasks.Task;
    using static Common.EventIds;
    using static Microsoft.Extensions.Logging.LoggerMessage;
    using static Microsoft.Extensions.Logging.LogLevel;

    /// <summary>A notification sent when a <see cref="UpdateRangeRequest{T}"/> is started.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class UpdateRangeNotification<T> : INotification, ILoggable
    {
        private static readonly Action<ILogger, Expression, T, Exception> LogAction = Define<Expression, T>(Information, UpdateRangeStart, "{@Expression}, {@Model}");

        private readonly IDictionary<Expression<Func<T, bool>>, T> _keyValuePairs;
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="UpdateRangeNotification{T}"/> class.</summary>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="keyValuePairs"/> is <see langword="null" />
        /// or
        /// <paramref name="logger"/> is <see langword="null" />.</exception>
        public UpdateRangeNotification(IDictionary<Expression<Func<T, bool>>, T> keyValuePairs, ILogger logger)
        {
            _keyValuePairs = keyValuePairs ?? throw new ArgumentNullException(nameof(keyValuePairs));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            foreach (var keyValuePair in _keyValuePairs)
            {
                LogAction(_logger, keyValuePair.Key.Body, keyValuePair.Value, default);
            }

            return CompletedTask;
        }
    }
}
