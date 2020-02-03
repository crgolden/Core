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
    using static DeleteRangeNotification;

    /// <summary>A notification sent when a <see cref="DeleteRangeRequest{T}"/> is started.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class DeleteRangeNotification<T> : INotification, ILoggable
        where T : class
    {
        private readonly IReadOnlyCollection<Expression<Func<T, bool>>> _expressions;
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="DeleteRangeNotification{T}"/> class.</summary>
        /// <param name="expressions">The expressions.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expressions"/> is <see langword="null" />
        /// or
        /// <paramref name="logger"/> is <see langword="null" />.</exception>
        public DeleteRangeNotification(IReadOnlyCollection<Expression<Func<T, bool>>> expressions, ILogger logger)
        {
            _expressions = expressions ?? throw new ArgumentNullException(nameof(expressions));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            foreach (var expression in _expressions)
            {
                StartAction(_logger, expression.Body, default);
            }

            return CompletedTask;
        }
    }
}
