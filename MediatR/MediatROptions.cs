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
        /// <see langword="true"/> if set to use the <see cref="QueryRequestHandler{T}"/>; otherwise, <see langword="false"/>.</value>
        public bool UseQueryRequestHandler { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use the <see cref="CommandRequestHandler{T}"/>.</summary>
        /// <value>
        /// <see langword="true"/> if set to use the <see cref="CommandRequestHandler{T}"/>; otherwise, <see langword="false"/>.</value>
        public bool UseCommandRequestHandler { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use the <see cref="LoggableNotificationHandler{T}"/>.</summary>
        /// <value>
        /// <see langword="true"/> if set to use the <see cref="LoggableNotificationHandler{T}"/>; otherwise, <see langword="false"/>.</value>
        public bool UseLoggableNotificationHandler { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use the <see cref="LoggingPipelineBehavior{T}"/>.</summary>
        /// <value>
        /// <see langword="true"/> if set to use the <see cref="LoggingPipelineBehavior{T}"/>; otherwise, <see langword="false"/>.</value>
        public bool UseLoggingPipelineBehavior { get; set; } = true;

        /// <summary>Gets or sets a value indicating whether to use scoped logging.</summary>
        /// <value>
        /// <see langword="true"/> if set to use scoped logging; otherwise, <see langword="false"/>.</value>
        public bool UseScopedLogging { get; set; }
    }
}
