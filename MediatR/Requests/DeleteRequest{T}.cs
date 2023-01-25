namespace Core.Requests
{
    using System;
    using System.Linq.Expressions;
    using Common.Abilities;
    using Common.Services;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A command to delete a model.</summary>
    /// <typeparam name="T">The type of the model.</typeparam>
    [PublicAPI]
    public class DeleteRequest<T> : ScopeableRequest, IRequest, INameable
    {
        /// <summary>Initializes a new instance of the <see cref="DeleteRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="expression"/> is <see langword="null" />.</exception>
        public DeleteRequest(string name, Expression<Func<T, bool>> expression, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Expression = expression ?? throw new ArgumentNullException(nameof(expression));
        }

        /// <summary>Gets the name of the <see cref="IDataCommandService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataCommandService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the expression.</summary>
        /// <value>The expression.</value>
        public Expression<Func<T, bool>> Expression { get; }
    }
}
