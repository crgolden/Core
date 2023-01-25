namespace Core.NotificationHandlers
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Abilities;
    using JetBrains.Annotations;
    using MediatR;

    /// <summary>A handler for <see cref="ILoggable"/> notifications.</summary>
    /// <typeparam name="T">The type of the notification.</typeparam>
    [PublicAPI]
    public class LoggableNotificationHandler<T> : INotificationHandler<T>
        where T : INotification, ILoggable
    {
        /// <inheritdoc />
        public Task Handle(T notification, CancellationToken cancellationToken)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            return notification.Log();
        }
    }
}
