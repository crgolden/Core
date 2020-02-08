namespace Core.EntityTypeConfigurations
{
    using Entities;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <inheritdoc />
    [PublicAPI]
    public class UserClaimConfiguration : IEntityTypeConfiguration<UserClaim>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.ToTable("UserClaims");
        }
    }
}
