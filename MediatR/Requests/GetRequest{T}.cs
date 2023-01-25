namespace Core.Requests
{
    using System;
    using System.Collections.Generic;
    using Common.Abilities;
    using Common.Services;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A request to get a model.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class GetRequest<T> : ScopeableRequest, IRequest<T>, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="GetRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="keyValues">The key values.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="keyValues"/> is <see langword="null" />.</exception>
        public GetRequest(string name, IReadOnlyCollection<object> keyValues, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            KeyValues = keyValues ?? throw new ArgumentNullException(nameof(keyValues));
        }

        /// <summary>Gets the name of the <see cref="IDataQueryService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataQueryService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the key values.</summary>
        /// <value>The key values.</value>
        public IReadOnlyCollection<object> KeyValues { get; }
    }
}
