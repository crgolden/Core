namespace Core.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Common;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A command to delete a range of models.</summary>
    /// <typeparam name="T">The type of the models.</typeparam>
    [PublicAPI]
    public class DeleteRangeRequest<T> : ScopeableRequest, IRequest, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="DeleteRangeRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="expressions">The expressions.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="expressions"/> is <see langword="null" />.</exception>
        public DeleteRangeRequest(string name, IReadOnlyCollection<Expression<Func<T, bool>>> expressions, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Expressions = expressions ?? throw new ArgumentNullException(nameof(expressions));
        }

        /// <summary>Gets the name of the <see cref="IDataCommandService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataCommandService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the expressions.</summary>
        /// <value>The expressions.</value>
        public IReadOnlyCollection<Expression<Func<T, bool>>> Expressions { get; }
    }
}
