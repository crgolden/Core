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
    using static DeleteNotification;

    /// <summary>A notification sent when a <see cref="DeleteRequest{T}"/> is started.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class DeleteNotification<T> : INotification, ILoggable
    {
        private readonly Expression<Func<T, bool>> _expression;
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="DeleteNotification{T}"/> class.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null" />
        /// or
        /// <paramref name="logger"/> is <see langword="null" />.</exception>
        public DeleteNotification(Expression<Func<T, bool>> expression, ILogger logger)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            StartAction(_logger, _expression.Body, default);
            return CompletedTask;
        }
    }
}
