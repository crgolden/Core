namespace Microsoft.Extensions.DependencyInjection
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Security.Cryptography.X509Certificates;
    using AspNetCore.Authentication.Cookies;
    using AspNetCore.Authentication.Facebook;
    using AspNetCore.Builder;
    using AspNetCore.Identity;
    using Configuration;
    using Core;
    using Core.Entities;
    using IdentityServer4.AccessTokenValidation;
    using IdentityServer4.Configuration;
    using IdentityServer4.EntityFramework.Interfaces;
    using IdentityServer4.EntityFramework.Options;
    using JetBrains.Annotations;
    using Options;
    using static System.Convert;
    using static System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler;
    using static System.Security.Cryptography.X509Certificates.X509KeyStorageFlags;
    using static System.String;
    using static AspNetCore.Http.SameSiteMode;
    using static AspNetCore.Identity.IdentityConstants;
    using static IdentityServer4.AspNetIdentity.SecurityStampValidatorCallback;
    using static IdentityServer4.IdentityServerConstants;

    /// <summary>A class with methods that extend <see cref="IServiceCollection"/>.</summary>
    [PublicAPI]
    public static class ServiceCollectionExtensions
    {
        /// <summary>Adds the identity server.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <param name="isDevelopment">if set to <c>true</c> [is development].</param>
        /// <param name="identityOptionsAction">The identity options action.</param>
        /// <param name="identityServerOptionsAction">The identity server options action.</param>
        /// <param name="configurationStoreOptionsAction">The configuration store options action.</param>
        /// <param name="operationalStoreOptionsAction">The operational store options action.</param>
        /// <param name="authenticationOptionsAction">The authentication options action.</param>
        /// <param name="identityServerAuthenticationOptionsAction">The identity server authentication options action.</param>
        /// <param name="facebookOptionsAction">The Facebook options action.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>
        /// of <paramref name="config"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddIdentityServer(
            this IServiceCollection services,
            IConfigurationSection config,
            bool isDevelopment,
            Action<IdentityOptions> identityOptionsAction = default,
            Action<IdentityServer4.Configuration.IdentityServerOptions> identityServerOptionsAction = default,
            Action<ConfigurationStoreOptions> configurationStoreOptionsAction = default,
            Action<OperationalStoreOptions> operationalStoreOptionsAction = default,
            Action<AspNetCore.Authentication.AuthenticationOptions> authenticationOptionsAction = default,
            Action<IdentityServerAuthenticationOptions> identityServerAuthenticationOptionsAction = default,
            Action<FacebookOptions> facebookOptionsAction = default)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            services.Configure<Core.IdentityServerOptions>(config);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<Core.IdentityServerOptions>>().Value;
                return services.AddIdentityServer(
                    options,
                    isDevelopment,
                    identityServerOptionsAction,
                    configurationStoreOptionsAction,
                    operationalStoreOptionsAction,
                    identityOptionsAction,
                    authenticationOptionsAction,
                    identityServerAuthenticationOptionsAction,
                    facebookOptionsAction);
            }
        }

        /// <summary>Adds the identity server.</summary>
        /// <param name="services">The services.</param>
        /// <param name="configureOptions">The configure options.</param>
        /// <param name="isDevelopment">if set to <c>true</c> [is development].</param>
        /// <param name="identityOptionsAction">The identity options action.</param>
        /// <param name="identityServerOptionsAction">The identity server options action.</param>
        /// <param name="configurationStoreOptionsAction">The configuration store options action.</param>
        /// <param name="operationalStoreOptionsAction">The operational store options action.</param>
        /// <param name="authenticationOptionsAction">The authentication options action.</param>
        /// <param name="identityServerAuthenticationOptionsAction">The identity server authentication options action.</param>
        /// <param name="facebookOptionsAction">The Facebook options action.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>
        /// or
        /// <paramref name="configureOptions"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddIdentityServer(
            this IServiceCollection services,
            Action<Core.IdentityServerOptions> configureOptions,
            bool isDevelopment,
            Action<IdentityOptions> identityOptionsAction = default,
            Action<IdentityServer4.Configuration.IdentityServerOptions> identityServerOptionsAction = default,
            Action<ConfigurationStoreOptions> configurationStoreOptionsAction = default,
            Action<OperationalStoreOptions> operationalStoreOptionsAction = default,
            Action<AspNetCore.Authentication.AuthenticationOptions> authenticationOptionsAction = default,
            Action<IdentityServerAuthenticationOptions> identityServerAuthenticationOptionsAction = default,
            Action<FacebookOptions> facebookOptionsAction = default)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configureOptions == default)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            services.Configure(configureOptions);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<Core.IdentityServerOptions>>().Value;
                return services.AddIdentityServer(
                    options,
                    isDevelopment,
                    identityServerOptionsAction,
                    configurationStoreOptionsAction,
                    operationalStoreOptionsAction,
                    identityOptionsAction,
                    authenticationOptionsAction,
                    identityServerAuthenticationOptionsAction,
                    facebookOptionsAction);
            }
        }

        /// <summary>Adds the identity server.</summary>
        /// <param name="services">The services.</param>
        /// <param name="config">The config.</param>
        /// <param name="configureBinder">The configure binder.</param>
        /// <param name="isDevelopment">if set to <c>true</c> [is development].</param>
        /// <param name="identityOptionsAction">The identity options action.</param>
        /// <param name="identityServerOptionsAction">The identity server options action.</param>
        /// <param name="configurationStoreOptionsAction">The configuration store options action.</param>
        /// <param name="operationalStoreOptionsAction">The operational store options action.</param>
        /// <param name="authenticationOptionsAction">The authentication options action.</param>
        /// <param name="identityServerAuthenticationOptionsAction">The identity server authentication options action.</param>
        /// <param name="facebookOptionsAction">The Facebook options action.</param>
        /// <returns>The <paramref name="services"/>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="services"/> is <see langword="null"/>
        /// or
        /// <paramref name="config"/> is <see langword="null"/>
        /// or
        /// <paramref name="configureBinder"/> is <see langword="null"/>.</exception>
        public static IServiceCollection AddIdentityServer(
            this IServiceCollection services,
            IConfigurationSection config,
            Action<BinderOptions> configureBinder,
            bool isDevelopment,
            Action<IdentityOptions> identityOptionsAction = default,
            Action<IdentityServer4.Configuration.IdentityServerOptions> identityServerOptionsAction = default,
            Action<ConfigurationStoreOptions> configurationStoreOptionsAction = default,
            Action<OperationalStoreOptions> operationalStoreOptionsAction = default,
            Action<AspNetCore.Authentication.AuthenticationOptions> authenticationOptionsAction = default,
            Action<IdentityServerAuthenticationOptions> identityServerAuthenticationOptionsAction = default,
            Action<FacebookOptions> facebookOptionsAction = default)
        {
            if (services == default)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (config == default)
            {
                throw new ArgumentNullException(nameof(config));
            }

            if (configureBinder == default)
            {
                throw new ArgumentNullException(nameof(configureBinder));
            }

            services.Configure<Core.IdentityServerOptions>(config, configureBinder);
            using (var provider = services.BuildServiceProvider(true))
            {
                var options = provider.GetRequiredService<IOptions<Core.IdentityServerOptions>>().Value;
                return services.AddIdentityServer(
                    options,
                    isDevelopment,
                    identityServerOptionsAction,
                    configurationStoreOptionsAction,
                    operationalStoreOptionsAction,
                    identityOptionsAction,
                    authenticationOptionsAction,
                    identityServerAuthenticationOptionsAction,
                    facebookOptionsAction);
            }
        }

        [SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "Cannot dispose certs")]
        private static IServiceCollection AddIdentityServer(
            this IServiceCollection services,
            Core.IdentityServerOptions identityServerOptions,
            bool isDevelopment,
            Action<IdentityServer4.Configuration.IdentityServerOptions> identityServerOptionsAction,
            Action<ConfigurationStoreOptions> configurationStoreOptionsAction,
            Action<OperationalStoreOptions> operationalStoreOptionsAction,
            Action<IdentityOptions> identityOptionsAction,
            Action<AspNetCore.Authentication.AuthenticationOptions> authenticationOptionsAction,
            Action<IdentityServerAuthenticationOptions> identityServerAuthenticationOptionsAction,
            Action<FacebookOptions> facebookOptionsAction)
        {
            DefaultInboundClaimTypeMap.Clear();
            DefaultOutboundClaimTypeMap.Clear();

            var builder = new IdentityServerBuilder(services);
            identityServerOptionsAction = identityServerOptionsAction ?? (_ =>
            {
            });
            builder.Services.Configure<SecurityStampValidatorOptions>(opts =>
            {
                opts.OnRefreshingPrincipal = UpdatePrincipal;
            });
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = None;
            });
            builder.Services.ConfigureExternalCookie(options =>
            {
                options.Cookie.IsEssential = true;
                options.Cookie.SameSite = None;
            });
            builder.Services.Configure<CookieAuthenticationOptions>(TwoFactorRememberMeScheme, options =>
            {
                options.Cookie.IsEssential = true;
            });
            builder.Services.Configure<CookieAuthenticationOptions>(TwoFactorUserIdScheme, options =>
            {
                options.Cookie.IsEssential = true;
            });
            builder.Services.Configure(identityServerOptionsAction);
            builder.Services.AddIdentityServer();
            if (identityServerOptions.UseEntityFramework)
            {
                builder.Services.AddEntityFramework(configurationStoreOptionsAction, operationalStoreOptionsAction);
                builder.AddConfigurationStore<IdentityServerDbContext>(configurationStoreOptionsAction);
                builder.AddOperationalStore<IdentityServerDbContext>(operationalStoreOptionsAction);
            }

            if (identityServerOptions.UseIdentity)
            {
                builder.Services.AddIdentity(identityServerOptions, identityOptionsAction);
                builder.AddAspNetIdentity<User>();
            }

            if (identityServerOptions.UseAuthentication)
            {
                builder.Services.AddAuthentication(
                    identityServerOptions,
                    authenticationOptionsAction,
                    identityServerAuthenticationOptionsAction,
                    facebookOptionsAction);
            }

            if (isDevelopment)
            {
                builder.AddDeveloperSigningCredential();
            }
            else
            {
                if (!IsNullOrWhiteSpace(identityServerOptions.SigningCredential))
                {
                    var rawData = FromBase64String(identityServerOptions.SigningCredential);
                    var cert = new X509Certificate2(rawData, default(string), MachineKeySet);
                    builder.AddSigningCredential(cert);
                }

                if (!IsNullOrWhiteSpace(identityServerOptions.ValidationKey))
                {
                    var rawData = FromBase64String(identityServerOptions.ValidationKey);
                    var cert = new X509Certificate2(rawData, default(string), MachineKeySet);
                    builder.AddValidationKey(cert);
                }

                builder.AddConfigurationStoreCache();
            }

            return builder.Services;
        }

        private static IServiceCollection AddIdentity(
            this IServiceCollection services,
            Core.IdentityServerOptions options,
            Action<IdentityOptions> identityOptionsAction)
        {
            identityOptionsAction = identityOptionsAction ?? (_ =>
            {
            });
            var builder = services.AddIdentity<User, Role>(identityOptionsAction);
            if (options.UseEntityFramework)
            {
                builder.AddEntityFrameworkStores<IdentityServerDbContext>();
            }

            builder.AddDefaultTokenProviders();
            return builder.Services;
        }

        private static IServiceCollection AddEntityFramework(
            this IServiceCollection services,
            Action<ConfigurationStoreOptions> configurationStoreOptionsAction,
            Action<OperationalStoreOptions> operationalStoreOptionsAction)
        {
            services.AddScoped<IConfigurationDbContext, IdentityServerDbContext>();
            services.AddScoped<IPersistedGrantDbContext, IdentityServerDbContext>();
            configurationStoreOptionsAction = configurationStoreOptionsAction ?? (_ =>
            {
            });
            operationalStoreOptionsAction = operationalStoreOptionsAction ?? (_ =>
            {
            });
            services.Configure(configurationStoreOptionsAction);
            services.Configure(operationalStoreOptionsAction);
            return services;
        }

        private static IServiceCollection AddAuthentication(
            this IServiceCollection services,
            Core.IdentityServerOptions options,
            Action<AspNetCore.Authentication.AuthenticationOptions> authenticationOptionsAction,
            Action<IdentityServerAuthenticationOptions> identityServerAuthenticationOptionsAction,
            Action<FacebookOptions> facebookOptionsAction)
        {
            authenticationOptionsAction = authenticationOptionsAction ?? (authenticationOptions =>
            {
                if (!IsNullOrWhiteSpace(options.DefaultScheme))
                {
                    authenticationOptions.DefaultScheme = options.DefaultScheme;
                }

                if (authenticationOptions.DefaultAuthenticateScheme == default &&
                    authenticationOptions.DefaultScheme == DefaultCookieAuthenticationScheme)
                {
                    authenticationOptions.DefaultScheme = ApplicationScheme;
                }
            });
            var builder = services.AddAuthentication(authenticationOptionsAction);
            if (options.UseIdentityServer4Authentication)
            {
                identityServerAuthenticationOptionsAction = identityServerAuthenticationOptionsAction ?? (_ =>
                {
                });
                builder.AddIdentityServerAuthentication(identityServerAuthenticationOptionsAction);
            }

            if (options.UseFacebookAuthentication)
            {
                facebookOptionsAction = facebookOptionsAction ?? (_ =>
                {
                });
                builder.AddFacebook(facebookOptionsAction);
            }

            return builder.Services;
        }
    }
}
