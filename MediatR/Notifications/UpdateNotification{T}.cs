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
    using static Microsoft.Extensions.Logging.LoggerMessage;
    using static Microsoft.Extensions.Logging.LogLevel;

    /// <summary>A notification sent when a <see cref="UpdateRequest{T}"/> is started.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class UpdateNotification<T> : INotification, ILoggable
        where T : class
    {
        private static readonly Action<ILogger, Expression, T, Exception> LogAction = Define<Expression, T>(Information, UpdateStart, "{@Expression}, {@Model}");

        private readonly Expression<Func<T, bool>> _expression;
        private readonly T _model;
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="UpdateNotification{T}"/> class.</summary>
        /// <param name="expression">The expression.</param>
        /// <param name="model">The model.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="expression"/> is <see langword="null" />
        /// or
        /// <paramref name="model"/> is <see langword="null" />
        /// or
        /// <paramref name="logger"/> is <see langword="null" />.</exception>
        public UpdateNotification(Expression<Func<T, bool>> expression, T model, ILogger logger)
        {
            _expression = expression ?? throw new ArgumentNullException(nameof(expression));
            _model = model ?? throw new ArgumentNullException(nameof(model));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            LogAction(_logger, _expression.Body, _model, default);
            return CompletedTask;
        }
    }
}
