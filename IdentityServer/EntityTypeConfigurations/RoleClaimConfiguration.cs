namespace Core.EntityTypeConfigurations
{
    using Entities;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <inheritdoc />
    [PublicAPI]
    public class RoleClaimConfiguration : IEntityTypeConfiguration<RoleClaim>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<RoleClaim> builder)
        {
            builder.ToTable("RoleClaims");
        }
    }
}
