namespace Core.EntityTypeConfigurations
{
    using Entities;
    using JetBrains.Annotations;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    /// <inheritdoc />
    [PublicAPI]
    public class UserLoginConfiguration : IEntityTypeConfiguration<UserLogin>
    {
        /// <inheritdoc />
        public void Configure(EntityTypeBuilder<UserLogin> builder)
        {
            builder.ToTable("UserLogins");
        }
    }
}
