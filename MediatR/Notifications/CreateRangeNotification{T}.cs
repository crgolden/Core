namespace Core.Notifications
{
    using System;
    using System.Collections.Generic;
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

    /// <summary>A notification sent when a <see cref="CreateRangeRequest{T}"/> is started.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class CreateRangeNotification<T> : INotification, ILoggable
        where T : class
    {
        private static readonly Action<ILogger, T, Exception> LogAction = Define<T>(Information, CreateRangeStart, "{@Model}");

        private readonly IReadOnlyCollection<T> _models;
        private readonly ILogger _logger;

        /// <summary>Initializes a new instance of the <see cref="CreateRangeNotification{T}"/> class.</summary>
        /// <param name="models">The models.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="models"/> is <see langword="null" />
        /// or
        /// <paramref name="logger"/> is <see langword="null" />.</exception>
        public CreateRangeNotification(IReadOnlyCollection<T> models, ILogger logger)
        {
            _models = models ?? throw new ArgumentNullException(nameof(models));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc />
        public Task Log()
        {
            foreach (var model in _models)
            {
                LogAction(_logger, model, default);
            }

            return CompletedTask;
        }
    }
}
