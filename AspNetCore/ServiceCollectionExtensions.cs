namespace Core
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using static System.Security.Claims.ClaimTypes;

    public static class ServiceCollectionExtensions
    {
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
