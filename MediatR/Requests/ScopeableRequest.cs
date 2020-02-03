namespace Core.Requests
{
    using System;
    using Common;
    using JetBrains.Annotations;
    using Microsoft.Extensions.Logging;
    using static System.Guid;
    using static Microsoft.Extensions.Logging.LoggerMessage;

    /// <summary>A request that defines a logging scope.</summary>
    [PublicAPI]
    public abstract class ScopeableRequest : IScopeable
    {
        private static readonly Func<ILogger, Guid, IDisposable> ScopeFunction = DefineScope<Guid>("{Scope}");

        /// <summary>Initializes a new instance of the <see cref="ScopeableRequest"/> class.</summary>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="logger"/> is <see langword="null" />.</exception>
        protected ScopeableRequest(ILogger logger) => Logger = logger ?? throw new ArgumentNullException(nameof(logger));

        /// <summary>Gets the logger.</summary>
        /// <value>The logger.</value>
        public ILogger Logger { get; }

        /// <inheritdoc />
        public IDisposable Scope() => ScopeFunction(Logger, NewGuid());
    }
}
