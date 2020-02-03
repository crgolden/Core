namespace Core
{
    using JetBrains.Annotations;
    using MediatR;
    using NotificationHandlers;
    using PipelineBehaviors;
    using RequestHandlers;

    /// <summary>Configuration settings for the <see cref="IMediator"/> instance.</summary>
    [PublicAPI]
    public class MediatROptions
    {
        /// <summary>Gets or sets a value indicating whether to use the <see cref="QueryRequestHandler{T}"/>.</summary>
        /// <value>
        /// <c>true</c> if set to use the <see cref="QueryRequestHandler{T}"/>; otherwise, <c>false</c>.</value>
        public bool UseQueryRequestHandler { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use the <see cref="CommandRequestHandler{T}"/>.</summary>
        /// <value>
        /// <c>true</c> if set to use the <see cref="CommandRequestHandler{T}"/>; otherwise, <c>false</c>.</value>
        public bool UseCommandRequestHandler { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use the <see cref="LoggableNotificationHandler{T}"/>.</summary>
        /// <value>
        /// <c>true</c> if set to use the <see cref="LoggableNotificationHandler{T}"/>; otherwise, <c>false</c>.</value>
        public bool UseLoggableNotificationHandler { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use the <see cref="LoggingPipelineBehavior{T}"/>.</summary>
        /// <value>
        /// <c>true</c> if set to use the <see cref="LoggingPipelineBehavior{T}"/>; otherwise, <c>false</c>.</value>
        public bool UseLoggingPipelineBehavior { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use scoped logging.</summary>
        /// <value>
        /// <c>true</c> if set to use scoped logging.</value>
        public bool UseScopedLogging { get; set; }
    }
}
