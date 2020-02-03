namespace Microsoft.Extensions.DependencyInjection
{
    using AspNetCore.Builder;
    using Configuration;
    using JetBrains.Annotations;
    using static System.Security.Claims.ClaimTypes;

    /// <summary>A class with methods that extend <see cref="IServiceCollection"/>.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds the Identity Server authentication to <paramref name="services"/>.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="apiName">Name of the API.</param>
        /// <param name="defaultScheme">The default scheme.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        public static IServiceCollection AddIdentityServerAuthentication(
            this IServiceCollection services,
            IConfiguration configuration,
            string apiName,
            string defaultScheme = "Bearer")
        {
            services
                .AddAuthentication(defaultScheme)
                .AddIdentityServerAuthentication(
                    authenticationScheme: defaultScheme,
                    configureOptions: options =>
                    {
                        var identityServerAddress = configuration.GetValue<string>("IdentityServerAddress");
                        if (string.IsNullOrEmpty(identityServerAddress))
                        {
                            return;
                        }

                        options.Authority = identityServerAddress;
                        options.ApiName = apiName;
                        options.RoleClaimType = Role;
                    });
            return services;
        }
    }
}
