namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Configuration;
    using Core;
    using Core.NotificationHandlers;
    using Core.Notifications;
    using Core.PipelineBehaviors;
    using Core.RequestHandlers;
    using Core.Requests;
    using JetBrains.Annotations;
    using MediatR;
    using Options;
    using static System.Reflection.Assembly;

    /// <summary>A class with methods that extend <see cref="IServiceCollection"/>.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds an <see cref="IMediator"/> instance to <paramref name="services"/> using the provided <paramref name="modelTypes"/>, <paramref name="config"/>, and <paramref name="configureService"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="modelTypes">The model types.</param>
        /// <param name="config">The config.</param>
        /// <param name="configureService">The configure service.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="modelTypes"/> is <see langword="null" />
        /// or
        /// <paramref name="config"/> is <see langword="null" />
        /// or
        /// <paramref name="configureService"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddMediatR(
            this IServiceCollection services,
            IReadOnlyCollection<Type> modelTypes,
            IConfigurationSection config,
            Action<MediatRServiceConfiguration> configureService = default)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (modelTypes == null)
            {
                throw new ArgumentNullException(nameof(modelTypes));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.Configure<MediatROptions>(config);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<MediatROptions>>().Value;
                return AddMediatR(services, modelTypes, options, configureService);
            }
        }

        /// <summary>Adds an <see cref="IMediator"/> instance to <paramref name="services"/> using the provided <paramref name="modelTypes"/>, <paramref name="configureOptions"/>, and <paramref name="configureService"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="modelTypes">The model types.</param>
        /// <param name="configureOptions">The configure options.</param>
        /// <param name="configureService">The configure service.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="modelTypes"/> is <see langword="null" />
        /// or
        /// <paramref name="configureOptions"/> is <see langword="null" />
        /// or
        /// <paramref name="configureService"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddMediatR(
            this IServiceCollection services,
            IReadOnlyCollection<Type> modelTypes,
            Action<MediatROptions> configureOptions,
            Action<MediatRServiceConfiguration> configureService = default)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (modelTypes == null)
            {
                throw new ArgumentNullException(nameof(modelTypes));
            }

            if (configureOptions == default)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<MediatROptions>>().Value;
                return AddMediatR(services, modelTypes, options, configureService);
            }
        }

        /// <summary>Adds an <see cref="IMediator"/> instance to <paramref name="services"/> using the provided <paramref name="modelTypes"/>, <paramref name="config"/>, <paramref name="configureBinder"/>, and <paramref name="configureService"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="modelTypes">The model types.</param>
        /// <param name="config">The configuration.</param>
        /// <param name="configureBinder">The configure binder.</param>
        /// <param name="configureService">The configure service.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null" />
        /// or
        /// <paramref name="modelTypes"/> is <see langword="null" />
        /// or
        ///  <paramref name="config"/> is <see langword="null" />
        /// or
        /// <paramref name="configureBinder"/> is <see langword="null" />
        /// or
        /// <paramref name="configureService"/> is <see langword="null" />.</exception>
        public static IServiceCollection AddMediatR(
            this IServiceCollection services,
            IReadOnlyCollection<Type> modelTypes,
            IConfigurationSection config,
            Action<BinderOptions> configureBinder,
            Action<MediatRServiceConfiguration> configureService = default)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (modelTypes == null)
            {
                throw new ArgumentNullException(nameof(modelTypes));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (configureBinder == default)
            {
                throw new ArgumentNullException(nameof(configureBinder));
            }

            services.Configure<MediatROptions>(config, configureBinder);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<MediatROptions>>().Value;
                return AddMediatR(services, modelTypes, options, configureService);
            }
        }

        private static IServiceCollection AddMediatR(
            this IServiceCollection services,
            IReadOnlyCollection<Type> modelTypes,
            MediatROptions options,
            Action<MediatRServiceConfiguration> configureService)
        {
            foreach (var modelType in modelTypes.Where(x => x.IsClass).Distinct())
            {
                services.SetupQueries(modelType, options).SetupCommands(modelType, options);
            }

            services.AddMediatR(new[] { GetExecutingAssembly() }, configureService);
            var descriptor = services.Single(x => x.ImplementationType == Types.LoggableNotificationHandler);
            services.Remove(descriptor);
            foreach (var modelType in modelTypes.Where(x => x.IsClass).Distinct())
            {
                services.SetupNotifications(modelType, options);
            }

            return services;
        }

        private static IServiceCollection SetupQueries(this IServiceCollection services, Type modelType, MediatROptions options)
        {
            if (!options.UseQueryRequestHandler &&
                !options.UseLoggingPipelineBehavior)
            {
                return services;
            }

            var queryRequestHandler = Types.QueryRequestHandler.MakeGenericType(modelType);
            var loggingBehavior = Types.LoggingPipelineBehavior.MakeGenericType(modelType);
            if (options.UseQueryRequestHandler)
            {
                var queryType = Types.Queryable.MakeGenericType(modelType);
                var queryRequest = Types.QueryRequest.MakeGenericType(modelType);
                var requestHandler = Types.RequestHandler.MakeGenericType(queryRequest, queryType);
                services.AddScoped(requestHandler, queryRequestHandler);
            }

            var listType = Types.List.MakeGenericType(modelType);
            var getRangeRequest = Types.GetRangeRequest.MakeGenericType(modelType);
            if (options.UseQueryRequestHandler)
            {
                var requestHandler = Types.RequestHandler.MakeGenericType(getRangeRequest, listType);
                services.AddScoped(requestHandler, queryRequestHandler);
            }

            if (options.UseLoggingPipelineBehavior)
            {
                var behavior = Types.PipelineBehavior.MakeGenericType(getRangeRequest, listType);
                services.AddScoped(behavior, loggingBehavior);
            }

            var getRequest = Types.GetRequest.MakeGenericType(modelType);
            if (options.UseQueryRequestHandler)
            {
                var requestHandler = Types.RequestHandler.MakeGenericType(getRequest, modelType);
                services.AddScoped(requestHandler, queryRequestHandler);
            }

            if (options.UseLoggingPipelineBehavior)
            {
                var behavior = Types.PipelineBehavior.MakeGenericType(getRequest, modelType);
                services.AddScoped(behavior, loggingBehavior);
            }

            return services;
        }

        private static IServiceCollection SetupCommands(this IServiceCollection services, Type modelType, MediatROptions options)
        {
            if (!options.UseCommandRequestHandler &&
                !options.UseLoggingPipelineBehavior)
            {
                return services;
            }

            foreach (var type in Types.CommandRequests)
            {
                AddCommandType(type);
            }

            void AddCommandType(Type type)
            {
                var request = type.MakeGenericType(modelType);
                if (options.UseCommandRequestHandler)
                {
                    var commandRequestHandler = Types.CommandRequestHandler.MakeGenericType(modelType);
                    var requestHandler = Types.RequestHandler.MakeGenericType(request, Types.Unit);
                    services.AddScoped(requestHandler, commandRequestHandler);
                }

                if (options.UseLoggingPipelineBehavior)
                {
                    var loggingBehavior = Types.LoggingPipelineBehavior.MakeGenericType(modelType);
                    var behavior = Types.PipelineBehavior.MakeGenericType(request, Types.Unit);
                    services.AddScoped(behavior, loggingBehavior);
                }
            }

            return services;
        }

        private static IServiceCollection SetupNotifications(this IServiceCollection services, Type modelType, MediatROptions options)
        {
            if (!options.UseLoggableNotificationHandler)
            {
                return services;
            }

            var notifications = new List<Type>(Types.Notifications);
            notifications.AddRange(Types.GenericNotifications.Select(type => type.MakeGenericType(modelType)));

            foreach (var notification in notifications)
            {
                var loggableNotificationHandler = Types.LoggableNotificationHandler.MakeGenericType(notification);
                var notificationHandler = Types.NotificationHandler.MakeGenericType(notification);
                services.AddScoped(notificationHandler, loggableNotificationHandler);
            }

            return services;
        }

        private static class Types
        {
            internal static readonly Type Queryable = typeof(IQueryable<>);
            internal static readonly Type List = typeof(List<>);
            internal static readonly Type Unit = typeof(Unit);

            // Request Handlers
            internal static readonly Type RequestHandler = typeof(IRequestHandler<,>);
            internal static readonly Type QueryRequestHandler = typeof(QueryRequestHandler<>);
            internal static readonly Type CommandRequestHandler = typeof(CommandRequestHandler<>);

            // Requests
            internal static readonly Type QueryRequest = typeof(QueryRequest<>);
            internal static readonly Type GetRequest = typeof(GetRequest<>);
            internal static readonly Type GetRangeRequest = typeof(GetRangeRequest<>);

            internal static readonly Type[] CommandRequests =
            {
                typeof(CreateRequest<>),
                typeof(CreateRangeRequest<>),
                typeof(UpdateRequest<>),
                typeof(UpdateRangeRequest<>),
                typeof(DeleteRequest<>),
                typeof(DeleteRangeRequest<>)
            };

            // Notification Handlers
            internal static readonly Type NotificationHandler = typeof(INotificationHandler<>);
            internal static readonly Type LoggableNotificationHandler = typeof(LoggableNotificationHandler<>);

            // Notifications
            internal static readonly Type[] Notifications =
            {
                typeof(CreateNotification),
                typeof(CreateRangeNotification),
                typeof(DeleteNotification),
                typeof(DeleteRangeNotification),
                typeof(GetNotification),
                typeof(GetRangeNotification),
                typeof(UpdateNotification),
                typeof(UpdateRangeNotification)
            };

            internal static readonly Type[] GenericNotifications =
            {
                typeof(CreateNotification<>),
                typeof(CreateRangeNotification<>),
                typeof(DeleteNotification<>),
                typeof(DeleteRangeNotification<>),
                typeof(GetNotification<>),
                typeof(GetRangeNotification<>),
                typeof(UpdateNotification<>),
                typeof(UpdateRangeNotification<>)
            };

            // Pipeline Behaviors
            internal static readonly Type PipelineBehavior = typeof(IPipelineBehavior<,>);
            internal static readonly Type LoggingPipelineBehavior = typeof(LoggingPipelineBehavior<>);
        }
    }
}
