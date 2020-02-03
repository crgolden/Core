namespace Core.Requests
{
    using System;
    using System.Linq.Expressions;
    using Common;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A command to update a model.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class UpdateRequest<T> : ScopeableRequest, IRequest, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="UpdateRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="model">The model.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="expression"/> is <see langword="null" />
        /// or
        /// <paramref name="model"/> is <see langword="null" />.</exception>
        public UpdateRequest(string name, Expression<Func<T, bool>> expression, T model, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
            Model = model ?? throw new ArgumentNullException(nameof(model));
        }

        /// <summary>Gets the name of the <see cref="IDataCommandService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataCommandService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public Expression<Func<T, bool>> Expression { get; }

        /// <summary>Gets the model.</summary>
        /// <value>The model.</value>
        public T Model { get; }
    }
}
