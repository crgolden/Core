namespace Core.Requests
{
    using System;
    using Common.Abilities;
    using Common.Services;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A command to create a model.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class CreateRequest<T> : ScopeableRequest, IRequest, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="CreateRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="model">The model.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="model"/> is <see langword="null" />.</exception>
        public CreateRequest(string name, T model, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        /// <summary>Gets the name of the <see cref="IDataCommandService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataCommandService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the model.</summary>
        /// <value>The model.</value>
        public T Model { get; }
    }
}
