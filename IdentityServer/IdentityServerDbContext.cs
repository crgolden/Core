namespace Core
{
    using System;
    using System.Threading.Tasks;
    using Entities;
    using EntityTypeConfigurations;
    using IdentityServer4.EntityFramework.Entities;
    using IdentityServer4.EntityFramework.Extensions;
    using IdentityServer4.EntityFramework.Interfaces;
    using IdentityServer4.EntityFramework.Options;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;

    /// <inheritdoc cref="IdentityDbContext" />
    [PublicAPI]
    public class IdentityServerDbContext : IdentityDbContext<User, Role, Guid, Entities.UserClaim, UserRole, UserLogin, RoleClaim, UserToken>,
        IConfigurationDbContext,
        IPersistedGrantDbContext
    {
        /// <summary>Initializes a new instance of the <see cref="IdentityServerDbContext"/> class.</summary>
        /// <param name="options">The options.</param>
        /// <param name="configurationStoreOptions">The configuration store options.</param>
        /// <param name="operationalStoreOptions">The operational store options.</param>
        /// <exception cref="ArgumentNullException"><paramref name="configurationStoreOptions"/> is <see langword="null"/>
        /// or
        /// <paramref name="operationalStoreOptions"/> is <see langword="null"/>.</exception>
        public IdentityServerDbContext(
            DbContextOptions<IdentityServerDbContext> options,
            IOptionsSnapshot<ConfigurationStoreOptions> configurationStoreOptions,
            IOptionsSnapshot<OperationalStoreOptions> operationalStoreOptions)
            : base(options)
        {
            ConfigurationStoreOptions = configurationStoreOptions?.Value ?? throw new ArgumentNullException(nameof(configurationStoreOptions));
            OperationalStoreOptions = operationalStoreOptions?.Value ?? throw new ArgumentNullException(nameof(operationalStoreOptions));
        }

        /// <inheritdoc />
        public virtual DbSet<Client> Clients { get; set; }

        /// <inheritdoc />
        public virtual DbSet<IdentityResource> IdentityResources { get; set; }

        /// <inheritdoc />
        public virtual DbSet<ApiResource> ApiResources { get; set; }

        /// <inheritdoc />
        public virtual DbSet<PersistedGrant> PersistedGrants { get; set; }

        /// <inheritdoc />
        public virtual DbSet<DeviceFlowCodes> DeviceFlowCodes { get; set; }

        /// <summary>Gets the configuration store options.</summary>
        /// <value>The configuration store options.</value>
        protected ConfigurationStoreOptions ConfigurationStoreOptions { get; }

        /// <summary>Gets the operational store options.</summary>
        /// <value>The operational store options.</value>
        protected OperationalStoreOptions OperationalStoreOptions { get; }

        /// <inheritdoc cref="IConfigurationDbContext.SaveChangesAsync" />
        /// <inheritdoc cref="IPersistedGrantDbContext.SaveChangesAsync" />
        public virtual Task<int> SaveChangesAsync() => base.SaveChangesAsync();

        /// <inheritdoc />
        protected override void OnModelCreating(ModelBuilder builder)
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            base.OnModelCreating(builder);
            builder.ApplyConfiguration(new RoleConfiguration());
            builder.ApplyConfiguration(new RoleClaimConfiguration());
            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new UserClaimConfiguration());
            builder.ApplyConfiguration(new UserLoginConfiguration());
            builder.ApplyConfiguration(new UserRoleConfiguration());
            builder.ApplyConfiguration(new UserTokenConfiguration());
            builder.ConfigureClientContext(ConfigurationStoreOptions);
            builder.ConfigureResourcesContext(ConfigurationStoreOptions);
            builder.ConfigurePersistedGrantContext(OperationalStoreOptions);
        }
    }
}
