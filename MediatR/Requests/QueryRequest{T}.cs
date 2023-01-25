namespace Core.Requests
{
    using System;
    using System.Linq;
    using Common.Abilities;
    using Common.Services;
    using JetBrains.Annotations;
    using MediatR;
    using static System.String;

    /// <summary>A request to get a query.</summary>
    /// <typeparam name="T">The type of the query.</typeparam>
    [PublicAPI]
    public class QueryRequest<T> : IRequest<IQueryable<T>>, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="QueryRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />.</exception>
        public QueryRequest(string name)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
        }

        /// <summary>Gets the name of the <see cref="IDataQueryService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataQueryService"/> to use for this request.</value>
        public string Name { get; }
    }
}
