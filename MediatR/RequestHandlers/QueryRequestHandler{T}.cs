namespace Core.RequestHandlers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Services;
    using JetBrains.Annotations;
    using MediatR;
    using Requests;
    using static System.StringComparison;
    using static System.Threading.Tasks.Task;

    /// <summary>A handler for requests of type <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">The type of the requests.</typeparam>
    [PublicAPI]
    public class QueryRequestHandler<T> :
        IRequestHandler<QueryRequest<T>, IQueryable<T>>,
        IRequestHandler<GetRequest<T>, T>,
        IRequestHandler<GetRangeRequest<T>, List<T>>
        where T : class
    {
        private readonly IEnumerable<IDataQueryService> _services;

        /// <summary>Initializes a new instance of the <see cref="QueryRequestHandler{T}"/> class.</summary>
        /// <param name="services">The services.</param>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />.</exception>
        public QueryRequestHandler(IEnumerable<IDataQueryService> services) =>
            _services = services ?? throw new ArgumentNullException(nameof(services));

        /// <inheritdoc />
        public Task<IQueryable<T>> Handle(QueryRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Query Service not found for '{request.Name}'");
            }

            return FromResult(service.Query<T>());
        }

        /// <inheritdoc />
        public Task<T> Handle(GetRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Query Service not found for '{request.Name}'");
            }

            return service.GetAsync<T>(request.KeyValues.ToArray(), cancellationToken).AsTask();
        }

        /// <inheritdoc />
        public Task<List<T>> Handle(GetRangeRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Query Service not found for '{request.Name}'");
            }

            return service.ToListAsync(request.Query, cancellationToken);
        }
    }
}
