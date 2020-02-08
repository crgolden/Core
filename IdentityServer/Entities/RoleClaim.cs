namespace Core.Entities
{
    using System;
    using JetBrains.Annotations;
    using Microsoft.AspNetCore.Identity;

    /// <inheritdoc />
    [PublicAPI]
    public class RoleClaim : IdentityRoleClaim<Guid>
    {
        /// <summary>Gets or sets the role.</summary>
        /// <value>The role.</value>
        public virtual Role Role { get; set; }
    }
}
