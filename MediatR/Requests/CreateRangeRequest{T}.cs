namespace Core.Requests
{
    using System;
    using System.Collections.Generic;
    using Common;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A command to delete a range of models.</summary>
    /// <typeparam name="T">The type of the models.</typeparam>
    [PublicAPI]
    public class CreateRangeRequest<T> : ScopeableRequest, IRequest, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="CreateRangeRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="models">The models.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="models"/> is <see langword="null" />.</exception>
        public CreateRangeRequest(string name, IReadOnlyCollection<T> models, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Models = models ?? throw new ArgumentNullException(nameof(models));
        }

        /// <summary>Gets the name of the <see cref="IDataCommandService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataCommandService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the models.</summary>
        /// <value>The models.</value>
        public IReadOnlyCollection<T> Models { get; }
    }
}
