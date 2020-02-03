namespace Core.Requests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Logging;
    using static System.String;

    /// <summary>A request for a range of models.</summary>
    /// <typeparam name="T">The type of the models.</typeparam>
    [PublicAPI]
    public class GetRangeRequest<T> : ScopeableRequest, IRequest<List<T>>, INameable
        where T : class
    {
        /// <summary>Initializes a new instance of the <see cref="GetRangeRequest{T}"/> class.</summary>
        /// <param name="name">The name.</param>
        /// <param name="query">The query.</param>
        /// <param name="logger">The logger.</param>
        /// <exception cref="ArgumentNullException"><paramref name="name"/> is <see langword="null" />
        /// or
        /// <paramref name="query"/> is <see langword="null" />.</exception>
        public GetRangeRequest(string name, IQueryable<T> query, ILogger logger)
            : base(logger)
        {
            if (IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Query = query ?? throw new ArgumentNullException(nameof(query));
        }

        /// <summary>Gets the name of the <see cref="IDataQueryService"/> to use for this request.</summary>
        /// <value>The name of the <see cref="IDataQueryService"/> to use for this request.</value>
        public string Name { get; }

        /// <summary>Gets the query.</summary>
        /// <value>The query.</value>
        public IQueryable<T> Query { get; }
    }
}
