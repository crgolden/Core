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
    using static MediatR.Unit;

    /// <summary>A handler for commands of type <typeparamref name="T"/>.</summary>
    /// <typeparam name="T">The type of the commands.</typeparam>
    [PublicAPI]
    public class CommandRequestHandler<T> :
        IRequestHandler<CreateRequest<T>>,
        IRequestHandler<CreateRangeRequest<T>>,
        IRequestHandler<UpdateRequest<T>>,
        IRequestHandler<UpdateRangeRequest<T>>,
        IRequestHandler<DeleteRequest<T>>,
        IRequestHandler<DeleteRangeRequest<T>>
        where T : class
    {
        private readonly IEnumerable<IDataCommandService> _services;

        /// <summary>Initializes a new instance of the <see cref="CommandRequestHandler{T}"/> class.</summary>
        /// <param name="services">The data command services.</param>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />.</exception>
        public CommandRequestHandler(IEnumerable<IDataCommandService> services) =>
            _services = services ?? throw new ArgumentNullException(nameof(services));

        /// <inheritdoc />
        public Task<Unit> Handle(CreateRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Command Service not found for '{request.Name}'");
            }

            async Task<Unit> Handle()
            {
                await service.CreateAsync(request.Model, cancellationToken).ConfigureAwait(false);
                await service.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Value;
            }

            return Handle();
        }

        /// <inheritdoc />
        public Task<Unit> Handle(CreateRangeRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Command Service not found for '{request.Name}'");
            }

            async Task<Unit> Handle()
            {
                await service.CreateRangeAsync(request.Models, cancellationToken).ConfigureAwait(false);
                await service.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Value;
            }

            return Handle();
        }

        /// <inheritdoc />
        public Task<Unit> Handle(UpdateRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Command Service not found for '{request.Name}'");
            }

            async Task<Unit> Handle()
            {
                await service.UpdateAsync(request.Expression, request.Model, cancellationToken).ConfigureAwait(false);
                await service.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Value;
            }

            return Handle();
        }

        /// <inheritdoc />
        public Task<Unit> Handle(UpdateRangeRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Command Service not found for '{request.Name}'");
            }

            async Task<Unit> Handle()
            {
                await service.UpdateRangeAsync(request.KeyValuePairs, cancellationToken).ConfigureAwait(false);
                await service.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Value;
            }

            return Handle();
        }

        /// <inheritdoc />
        public Task<Unit> Handle(DeleteRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Command Service not found for '{request.Name}'");
            }

            async Task<Unit> Handle()
            {
                await service.DeleteAsync(request.Expression, cancellationToken).ConfigureAwait(false);
                await service.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Value;
            }

            return Handle();
        }

        /// <inheritdoc />
        public Task<Unit> Handle(DeleteRangeRequest<T> request, CancellationToken cancellationToken)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var service = _services.SingleOrDefault(x => string.Equals(request.Name, x.Name, InvariantCultureIgnoreCase));
            if (service == default)
            {
                throw new InvalidOperationException($"Data Command Service not found for '{request.Name}'");
            }

            async Task<Unit> Handle()
            {
                await service.DeleteRangeAsync(request.Expressions, cancellationToken).ConfigureAwait(false);
                await service.SaveChangesAsync(cancellationToken).ConfigureAwait(false);
                return Value;
            }

            return Handle();
        }
    }
}
