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
    using static Microsoft.Extensions.Logging.LoggerMessage;
    using static Microsoft.Extensions.Logging.LogLevel;

    /// <summary>A notification sent when a <see cref="CreateRequest{T}"/> is started.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class CreateNotification<T> : INotification, ILoggable
        where T : class
    {
        private static readonly Action<ILogger, T, Exception> LogAction = Define<T>(Information, CreateStart, "{@Model}");

        private readonly T _model;
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="CreateNotification{T}"/> class.</summary>
        /// <param name="model">The model.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="model"/> is <see langword="null" />
        /// or
        /// <paramref name="logger"/> is <see langword="null" />.</exception>
        public CreateNotification(T model, ILogger logger)
        {
            _model = model;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            LogAction(_logger, _model, default);
            return CompletedTask;
        }
    }
}