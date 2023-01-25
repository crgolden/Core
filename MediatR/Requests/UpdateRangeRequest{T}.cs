namespace Core.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Common.Abilities;
    using Common.Services;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A command to update a range of models.</summary>
    /// <typeparam name="T">The type of the models.</typeparam>
    [PublicAPI]
    public class UpdateRangeRequest<T> : ScopeableRequest, IRequest, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="UpdateRangeRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="keyValuePairs">The key value pairs.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="keyValuePairs"/> is <see langword="null" />.</exception>
        public UpdateRangeRequest(string name, IDictionary<Expression<Func<T, bool>>, T> keyValuePairs, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            KeyValuePairs = keyValuePairs ?? throw new ArgumentNullException(nameof(keyValuePairs));
        }

        /// <summary>Gets the name of the <see cref="IDataCommandService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataCommandService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the key value pairs.</summary>
        /// <value>The key value pairs.</value>
        public IDictionary<Expression<Func<T, bool>>, T> KeyValuePairs { get; }
    }
}
