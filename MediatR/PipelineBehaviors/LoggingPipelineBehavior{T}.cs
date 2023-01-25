namespace Core.PipelineBehaviors
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using MediatR;
    using Microsoft.Extensions.Options;
    using Notifications;
    using Requests;

    /// <summary>A pipeline behavior that logs requests and responses.</summary>
    /// <typeparam name="T">The type of the responses.</typeparam>
    [PublicAPI]
    public class LoggingPipelineBehavior<T> :
        IPipelineBehavior<GetRequest<T>, T>,
        IPipelineBehavior<GetRangeRequest<T>, List<T>>,
        IPipelineBehavior<CreateRequest<T>, Unit>,
        IPipelineBehavior<CreateRangeRequest<T>, Unit>,
        IPipelineBehavior<UpdateRequest<T>, Unit>,
        IPipelineBehavior<UpdateRangeRequest<T>, Unit>,
        IPipelineBehavior<DeleteRequest<T>, Unit>,
        IPipelineBehavior<DeleteRangeRequest<T>, Unit>
        where T : class
    {
        private readonly IMediator _mediator;
        private readonly MediatROptions _options;

        /// <summary>Initializes a new instance of the <see cref="LoggingPipelineBehavior{T}"/> class.</summary>
        /// <param name="mediator">The mediator.</param>
        /// <param name="options">The options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mediator"/> is <see langword="null" />
        /// or
        /// <paramref name="options"/> is <see langword="null" />.</exception>
        public LoggingPipelineBehavior(IMediator mediator, IOptions<MediatROptions> options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        /// <inheritdoc />
        public Task<T> Handle(GetRequest<T> request, RequestHandlerDelegate<T> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<T> Handle()
            {
                var start = new GetNotification(request.KeyValues, request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new GetNotification<T>(response, request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }

        /// <inheritdoc />
        public Task<List<T>> Handle(GetRangeRequest<T> request, RequestHandlerDelegate<List<T>> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<List<T>> Handle()
            {
                var start = new GetRangeNotification(request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new GetRangeNotification<T>(response, request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }

        /// <inheritdoc />
        public Task<Unit> Handle(CreateRequest<T> request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<Unit> Handle()
            {
                var start = new CreateNotification<T>(request.Model, request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new CreateNotification(request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }

        /// <inheritdoc />
        public Task<Unit> Handle(CreateRangeRequest<T> request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<Unit> Handle()
            {
                var start = new CreateRangeNotification<T>(request.Models, request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new CreateRangeNotification(request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }

        /// <inheritdoc />
        public Task<Unit> Handle(UpdateRequest<T> request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<Unit> Handle()
            {
                var start = new UpdateNotification<T>(request.Expression, request.Model, request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new UpdateNotification(request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }

        /// <inheritdoc />
        public Task<Unit> Handle(UpdateRangeRequest<T> request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<Unit> Handle()
            {
                var start = new UpdateRangeNotification<T>(request.KeyValuePairs, request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new UpdateRangeNotification(request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }

        /// <inheritdoc />
        public Task<Unit> Handle(DeleteRequest<T> request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<Unit> Handle()
            {
                var start = new DeleteNotification<T>(request.Expression, request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new DeleteNotification(request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }

        /// <inheritdoc />
        public Task<Unit> Handle(DeleteRangeRequest<T> request, RequestHandlerDelegate<Unit> next, CancellationToken cancellationToken = default)
        {
            if (request == default)
            {
                throw new ArgumentNullException(nameof(request));
            }

            if (next == default)
            {
                throw new ArgumentNullException(nameof(next));
            }

            if (!_options.UseScopedLogging)
            {
                return Handle();
            }

            using (request.Scope)
            {
                return Handle();
            }

            async Task<Unit> Handle()
            {
                var start = new DeleteRangeNotification<T>(request.Expressions, request.Logger);
                await _mediator.Publish(start, cancellationToken).ConfigureAwait(false);
                var response = await next().ConfigureAwait(false);
                var finish = new DeleteRangeNotification(request.Logger);
                await _mediator.Publish(finish, cancellationToken).ConfigureAwait(false);
                return response;
            }
        }
    }
}
